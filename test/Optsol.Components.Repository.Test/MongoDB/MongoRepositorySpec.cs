using FluentAssertions;
using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;
using Optsol.Components.Repository.Infra.MongoDB.Repositories;
using Optsol.Components.Repository.Infra.MongoDB.Settings;
using Xunit;

namespace Optsol.Components.Repository.Test.MongoDB
{
    public class MongoRepositorySpec
    {
        public class Aggregate : AggregateRoot
        {
            public static Aggregate Create() => new();
        }

        public class MongoContext : Context
        {
            public MongoContext(MongoSettings mongoSettings, IMongoClient mongoClient)
                : base(mongoSettings, mongoClient)
            {
            }
        }

        [Fact]
        public void Deve()
        {
            //given
            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "repository-mongo",
                ConnectionString = "mongodb://127.0.0.1:30001",
            };
            
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);

            var context = new MongoContext(mongoSettings, mongoClient);

            var mongoRepository = new MongoRepository<Aggregate>(context);
            //when
            mongoRepository.Insert(new Aggregate());

            //then
            mongoRepository.Should().NotBeNull();
        }
    }
}
