using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;
using Optsol.Components.Repository.Infra.MongoDB.Settings;
using Xunit;

namespace Optsol.Components.Repository.Test.MongoDB
{
    public class ContextFact
    {
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
            var mongoSettingsMock = new Mock<MongoSettings>();
            var mongoClientMock = new Mock<IMongoClient>();

            //when
            var context = new MongoContext(mongoSettingsMock.Object, mongoClientMock.Object);

            //then
            context.Should().NotBeNull();
        }
    }
}
