using MongoDB.Bson;
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

        public virtual IEnumerable<TAggregateRoot> GetAll()
        {
            if (typeof(TAggregateRoot).GetInterfaces().Contains(typeof(IEntityDeletable)))
            {
                var deletableDef = Builders<TAggregateRoot>.Filter.Eq("DeletedDate.Date", BsonNull.Value);
                return Set.Find(deletableDef).ToEnumerable();
            }

            return Set.AsQueryable().ToEnumerable();
        }

        public virtual IEnumerable<TAggregateRoot> GetAllByIds(params Guid[] ids)
        {
            if (typeof(TAggregateRoot).GetInterfaces().Contains(typeof(IEntityDeletable)))
            {
                var byIdDef = Builders<TAggregateRoot>.Filter.In(q => q.Id, ids);
                var deletableDef = Builders<TAggregateRoot>.Filter.Eq("DeletedDate.Date", BsonNull.Value);
                var filterDef = Builders<TAggregateRoot>.Filter.And(byIdDef, deletableDef);

                return Set.Find(filterDef).ToEnumerable();
            }

            return Set.Find(f => ids.Contains(f.Id)).ToEnumerable();
        }

        public virtual TAggregateRoot GetById(Guid id)
        {
            if (typeof(TAggregateRoot).GetInterfaces().Contains(typeof(IEntityDeletable)))
            {
                var byIdDef = Builders<TAggregateRoot>.Filter.Eq(q => q.Id, id);
                var deletableDef = Builders<TAggregateRoot>.Filter.Eq("DeletedDate.Date", BsonNull.Value);
                var filterDef = Builders<TAggregateRoot>.Filter.And(byIdDef, deletableDef);
                return Set.Find(filterDef).FirstOrDefault();
            }

            return Set.Find(f => f.Id.Equals(id)).FirstOrDefault();
        }

        public virtual IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, bool>> filterExpression)
        {
            if (typeof(TAggregateRoot).GetInterfaces().Contains(typeof(IEntityDeletable)))
            {
                var expressionDef = Builders<TAggregateRoot>.Filter.Where(filterExpression);
                var deletableDef = Builders<TAggregateRoot>.Filter.Eq("DeletedDate.Date", BsonNull.Value);
                var filterDef = Builders<TAggregateRoot>.Filter.And(expressionDef, deletableDef);

                return Set.Find(filterDef).ToEnumerable();
            }

            return Set.Find(filterExpression).ToEnumerable();
        }

        public virtual SearchResult<TAggregateRoot> GetAll<TSearch>(SearchRequest<TSearch> searchRequest) where TSearch : class
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

            var filterDef = GetFilterDef(search);

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

        private static FilterDefinition<TAggregateRoot> GetFilterDef(ISearch<TAggregateRoot> search)
        {
            var defaultDef = Builders<TAggregateRoot>.Filter.Empty;

            var isDeletable = (typeof(TAggregateRoot).GetInterfaces().Contains(typeof(IEntityDeletable)));
            var deletableDef = Builders<TAggregateRoot>.Filter.Eq("DeletedDate.Date", BsonNull.Value);

            if (search is null)
            {
                if (isDeletable)
                    return Builders<TAggregateRoot>.Filter.And(defaultDef, deletableDef);

                return defaultDef;
            }

            var searchDef = search.Searcher().Invoke(Builders<TAggregateRoot>.Filter);

            if (isDeletable)
                return Builders<TAggregateRoot>.Filter.And(searchDef, deletableDef);

            return searchDef;
        }

        public virtual void Insert(TAggregateRoot aggregate) => Context.AddTransaction(() => Set.InsertOneAsync(aggregate));

        public virtual void InsertRange(List<TAggregateRoot> aggregates) => Context.AddTransaction(() => Set.InsertManyAsync(aggregates));

        public virtual void Update(TAggregateRoot aggregate) => Context.AddTransaction(() => Set.ReplaceOneAsync(f => f.Id.Equals(aggregate.Id), aggregate));

        public virtual void Delete(TAggregateRoot aggregate)
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

        public void DeleteRange(List<TAggregateRoot> aggregates)
        {
            foreach (var aggregate in aggregates)
            {
                Delete(aggregate);
            }
        }

        public virtual int SaveChanges() => Context.SaveChanges();
    }
}