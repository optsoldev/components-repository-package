using FluentAssertions;
using FluentValidation.Results;
using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Notifications;
using Optsol.Components.Repository.Domain.Repositories.Pagination;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;
using Optsol.Components.Repository.Infra.MongoDB.Repositories;
using Optsol.Components.Repository.Infra.MongoDB.Repositories.Pagination;
using Optsol.Components.Repository.Infra.MongoDB.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Xunit;

namespace Optsol.Components.Repository.Test.MongoDB
{
    public class MongoRepositorySpec
    {
        public abstract class Aggregate : AggregateRoot, INotification
        {
            public abstract bool CadastroCompleto { get; }

            public bool CadastroIncompleto => CadastroCompleto is false;

            public Aggregate()
            {
                ValidationResult = new ValidationResult();
            }

            public bool Valid => ValidationResult.IsValid;

            public bool Invalid => Valid is false;

            public ValidationResult ValidationResult { get; protected set; }
        }

        public class Teste : Aggregate
        {
            public Teste(string nome, Metodologia metodologia, StatusAvaliacao status)
            {
                Nome = nome;
                Metodologia = metodologia;
                Status = status;
            }

            public override bool CadastroCompleto => true;

            public string Nome { get; private set; }

            public Metodologia Metodologia { get; private set; }

            public StatusAvaliacao Status { get; private set; }
        }

        public enum TipoStatusAvaliacao
        {
            Aguardando,
            Publicado
        }

        public enum TipoMetodologia
        {
            [Description("NPS")] NPS,
            [Description("Outros")] OUTROS
        }

        public class Metodologia : ValueObject
        {
            public TipoMetodologia Tipo { get; private set; }

            public Metodologia(TipoMetodologia tipo)
            {
                Tipo = tipo;
            }

            public override IEnumerable<object> GetEqualityComponents()
            {
                yield return Tipo;
            }
        }

        public class StatusAvaliacao : ValueObject
        {
            #region Properties

            public TipoStatusAvaliacao Tipo { get; private set; }

            #endregion

            #region Constructors

            public StatusAvaliacao(TipoStatusAvaliacao tipo)
            {
                Tipo = tipo;
            }

            #endregion

            #region Methods


            public override IEnumerable<object> GetEqualityComponents()
            {
                yield return Tipo;
            }

            #endregion
        }

        public class MongoContext : Context
        {
            public MongoContext(MongoSettings mongoSettings, IMongoClient mongoClient)
                : base(mongoSettings, mongoClient)
            {
            }
        }

        [Trait("Repositories", "Métodos Escrita")]
        [Fact(DisplayName = "Deve inserir pelo repositório no mongodb", Skip = "Teste Local")]
        public void Deve_Inserir_Pelo_Repositorio()
        {
            //given
            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = "mongodb://127.0.0.1:30001",
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Teste>(context);

            //when
            mongoRepository.Insert(new Teste("Nome", new Metodologia(TipoMetodologia.NPS), new StatusAvaliacao(TipoStatusAvaliacao.Aguardando)));
            var count = context.SaveChanges();

            //then
            mongoRepository.Should().NotBeNull();
            count.Should().Be(1);
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve buscar todos pelo repositório no mongodb", Skip = "Teste Local")]
        public void Deve_Obter_Todos_Pelo_Repositorio()
        {
            //given
            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = "mongodb://127.0.0.1:30001",
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Teste>(context);

            //when
            var all = mongoRepository.GetAll();

            //then
            all.Should().NotBeNull();
            all.Should().NotBeEmpty();
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve buscar pelo repositório no mongodb", Skip = "Teste Local")]
        public void Deve_Obter_Pelo_Repositorio()
        {
            //given
            var id = Guid.Parse("4efaf95f-adaf-45db-9eae-e2fd0da0693c");

            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = "mongodb://127.0.0.1:30001",
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Teste>(context);

            //when
            var aggregate = mongoRepository.GetById(id);

            //then
            aggregate.Should().NotBeNull();
            aggregate.Id.Should().Be(id);
        }

        public class TesteSearchDto : ISearch<Teste>, IOrderBy<Teste>
        {
            public string Nome { get; private set; }

            public TesteSearchDto(string nome)
            {
                Nome = nome;
            }

            public Func<FilterDefinitionBuilder<Teste>, FilterDefinition<Teste>> Searcher()
            {
                return query => query.Empty;
            }

            public Func<SortDefinitionBuilder<Teste>, SortDefinition<Teste>> OrderBy()
            {
                return order => order.Descending(a => a.CreatedDate);
            }
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve buscar paginado pelo repositório no mongodb", Skip = "Teste Local")]
        public void Deve_Obter_Paginado_Pelo_Repositorio()
        {
            //given
            var search = new TesteSearchDto("");
            var request = new SearchRequest<TesteSearchDto>(search, 1, 5);

            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = "mongodb://127.0.0.1:30001",
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Teste>(context);

            //when
            var aggregate = mongoRepository.GetAll(request);

            //then
            aggregate.Should().NotBeNull();
            aggregate.Page.Should().Be(1);
            aggregate.PageSize.Should().Be(5);
            aggregate.TotalCount.Should().Be(10);
            aggregate.Items.Should().NotBeEmpty();
            aggregate.Items.Should().HaveCount(5);
        }
    }
}
