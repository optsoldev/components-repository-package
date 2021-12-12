using MongoDB.Driver;
using Optsol.Components.Repository.Infra.MongoDB.Exceptions;
using Optsol.Components.Repository.Infra.MongoDB.Settings;
using Optsol.Components.Repository.Infra.MongoDB.Transactions;
using System;

namespace Optsol.Components.Repository.Infra.MongoDB.Contexts
{
    public abstract class Context : IDisposable
    {
        private bool disposed = false;

        public IMongoClient MongoClient { get; private set; }

        public IMongoDatabase Database { get; private set; }

        public IClientSessionHandle Session { get; private set; }

        public TransactionList Transactions { get; private set; }

        protected Context(MongoSettings mongoSettings, IMongoClient mongoClient)
        {
            if (mongoSettings is null)
            {
                throw new MongoDBException($"{nameof(mongoSettings)} está nulo");
            }

            MongoClient = mongoClient ?? throw new MongoDBException($"{nameof(mongoClient)} está nulo");
            Database = mongoClient.GetDatabase(mongoSettings.DatabaseName);
        }

        public int SaveChanges()
        {
            var countSaveTasks = 0;
            using (Session = MongoClient.StartSession())
            {
                Session.StartTransaction();

                Transactions.Execute();
                countSaveTasks = Transactions.Transactions.Count;
                Transactions.Clear();

                Session.CommitTransaction();
            }

            return countSaveTasks;
        }

        public IMongoCollection<TAggregate> GetCollection<TAggregate>(string databaseName) => 
            Database.GetCollection<TAggregate>(databaseName);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed is not true && disposing)
            {
                Session?.Dispose();
            }
            disposed = true;
        }
    }
}
