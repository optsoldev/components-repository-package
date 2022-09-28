using MongoDB.Driver;
using Optsol.Components.Repository.Infra.MongoDB.Exceptions;
using Optsol.Components.Repository.Infra.MongoDB.Settings;
using Optsol.Components.Repository.Infra.MongoDB.Transactions;
using System;
using System.Threading.Tasks;

namespace Optsol.Components.Repository.Infra.MongoDB.Contexts
{
    public abstract class Context : IDisposable
    {
        private bool disposed;
        private IClientSessionHandle session;
        private readonly TransactionList transactions;

        private IMongoClient MongoClient { get; }

        private IMongoDatabase Database { get; }

        protected Context(MongoSettings mongoSettings, IMongoClient mongoClient)
        {
            if (mongoSettings is null)
            {
                throw new MongoDbException($"{nameof(mongoSettings)} está nulo");
            }

            transactions = TransactionList.Create();

            MongoClient = mongoClient ?? throw new MongoDbException($"{nameof(mongoClient)} está nulo");
            Database = mongoClient.GetDatabase(mongoSettings.DatabaseName);
        }

        public int SaveChanges()
        {
            int countSaveTasks;
            using (session = MongoClient.StartSession())
            {
                session.StartTransaction();

                transactions.Execute();
                countSaveTasks = transactions.Transactions.Count;
                transactions.Clear();

                session.CommitTransaction();
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
                session?.Dispose();
            }
            disposed = true;
        }

        public void AddTransaction(Func<Task> transaction) => transactions.AddTransaction(transaction);
    }
}
