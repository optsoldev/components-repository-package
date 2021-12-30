using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories.Pagination;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;
using Optsol.Components.Repository.Infra.MongoDB.Repositories.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.MongoDB.Repositories
{
    public class MongoRepository<TAggregateRoot> : IMongoRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public MongoRepository(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Set = context.GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
        }

        public Context Context { get; private set; }

        public IMongoCollection<TAggregateRoot> Set { get; private set; }

        public IEnumerable<TAggregateRoot> GetAll() => Set.AsQueryable().ToEnumerable();

        public IEnumerable<TAggregateRoot> GetAllByIds(params Guid[] ids) => Set.Find(f => ids.Contains(f.Id)).ToEnumerable();

        public TAggregateRoot GetById(Guid id) => Set.Find(f => f.Id.Equals(id)).FirstOrDefault();

        public IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, bool>> filterExpression) => Set.Find(filterExpression).ToEnumerable();

        public SearchResult<TAggregateRoot> GetAll<TSearch>(SearchRequest<TSearch> searchRequest) where TSearch : class
        {
            var search = searchRequest.Search as ISearch<TAggregateRoot>;
            var orderBy = searchRequest.Search as IOrderBy<TAggregateRoot>;
            
            var countFacet = AggregateFacet.Create("countFacet",
                PipelineDefinition<TAggregateRoot, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<TAggregateRoot>()
                }));

            var sortDef = orderBy is null
                ? Builders<TAggregateRoot>.Sort.Descending(d => d.CreatedDate)
                : orderBy.OrderBy().Invoke(Builders<TAggregateRoot>.Sort);

            var dataFacet = AggregateFacet.Create("dataFacet",
                PipelineDefinition<TAggregateRoot, TAggregateRoot>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(sortDef),
                    PipelineStageDefinitionBuilder.Skip<TAggregateRoot>((searchRequest.Page - 1) * (searchRequest.pageSize ?? 0)),
                    PipelineStageDefinitionBuilder.Limit<TAggregateRoot>(searchRequest.pageSize.Value)
                }));

            var filterDef = search is null
                ? Builders<TAggregateRoot>.Filter.Empty
                : search.Searcher().Invoke(Builders<TAggregateRoot>.Filter);

            var aggregation = Set.Aggregate()
                .Match(filterDef)
                .Facet(countFacet, dataFacet)
                .ToList();

            var count = aggregation.First()
                .Facets.First(x => x.Name.Equals("countFacet"))
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name.Equals("dataFacet"))
                .Output<TAggregateRoot>();

            return new SearchResult<TAggregateRoot>()
                .SetPage(searchRequest.Page)
                .SetPageSize(searchRequest.pageSize)
                .SetPaginatedItems(data)
                .SetTotalCount((int)count);
        }

        public void Insert(TAggregateRoot aggregate) => Context.AddTransaction(() => Set.InsertOneAsync(aggregate));

        public void Update(TAggregateRoot aggregate) => Context.AddTransaction(() => Set.ReplaceOneAsync(f => f.Id.Equals(aggregate.Id), aggregate));

        public void Delete(TAggregateRoot aggregate)
        {
            if (aggregate is null) return;

            if (aggregate is IEntityDeletable)
            {
                (aggregate as IEntityDeletable).Delete();

                Update(aggregate);

                return;
            }

            Context.AddTransaction(() => Set.DeleteOneAsync(f => f.Id.Equals(aggregate.Id)));
        }

        public int SaveChanges() => Context.SaveChanges();
    }
}
