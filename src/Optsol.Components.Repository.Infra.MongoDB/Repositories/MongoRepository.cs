using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Repository.Infra.MongoDB.Repositories
{
    public class MongoRepository<TAggregate> : IMongoRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
    {
        public MongoRepository(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Set = context.GetCollection<TAggregate>(typeof(TAggregate).Name);
        }

        public Context Context { get; private set; }

        public IMongoCollection<TAggregate> Set { get; private set; }

        public IEnumerable<TAggregate> GetAll() => Set.AsQueryable().ToEnumerable();

        public IEnumerable<TAggregate> GetAllByIds(params Guid[] ids) => Set.Find(f => ids.Contains(f.Id)).ToEnumerable();

        public TAggregate GetById(Guid id) => Set.Find(f => f.Id.Equals(id)).FirstOrDefault();

        public void Insert(TAggregate aggregate) => Context.AddTransaction(() => Set.InsertOneAsync(aggregate));

        public void Update(TAggregate aggregate) => Context.AddTransaction(() => Set.ReplaceOneAsync(f => f.Id.Equals(aggregate.Id), aggregate));

        public void Delete(TAggregate aggregate) => Context.AddTransaction(() => Set.DeleteOneAsync(f => f.Id.Equals(aggregate.Id)));

        public int SaveChanges() => Context.SaveChanges();
    }
}
