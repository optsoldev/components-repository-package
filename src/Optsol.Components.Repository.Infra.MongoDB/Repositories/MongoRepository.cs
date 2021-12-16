using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        public IEnumerable<TAggregate> GetAll()
        {
            var allRegistries = Set.Find(Builders<TAggregate>.Filter.Empty);
            return allRegistries.ToEnumerable();
        }

        public IEnumerable<TAggregate> GetAllByIds(params Guid[] ids)
        {
            var allRegistries = Set.Find(Builders<TAggregate>.Filter.All("_id", ids));
            return allRegistries.ToEnumerable();
        }

        public TAggregate GetById(Guid id)
        {
            var aggregate = Set.Find(Builders<TAggregate>.Filter.Eq("_id", id));
            return aggregate.SingleOrDefault();
        }

        public void Insert(TAggregate aggregate)
        {
            Context.Transactions.AddTransaction(() => Set.InsertOneAsync(aggregate));
        }

        public void Update(TAggregate aggregate)
        {
            Context.Transactions.AddTransaction(() => Set.ReplaceOneAsync(Builders<TAggregate>.Filter.Eq("_id", aggregate.Id), aggregate));
        }

        public void Delete(TAggregate aggregate)
        {
            Context.Transactions.AddTransaction(() => Set.DeleteOneAsync(Builders<TAggregate>.Filter.Eq("_id", aggregate.Id)));
        }

        public int SaveChanges() => Context.SaveChanges();
    }
}
