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
using Xunit;

namespace Optsol.Components.Repository.Test.MongoDB
{
    public class MongoRepositorySpec
    {
        public const string CONNECTION_STRING = "mongodb://127.0.0.1:30001";

        public abstract class Aggregate : AggregateRoot, INotification
        {
            public abstract bool CadastroCompleto { get; }

            public bool CadastroIncompleto => CadastroCompleto is false;

            public Aggregate()
            {
                ValidationResult = new ValidationResult();
            }

            public ValidationResult ValidationResult { get; protected set; }

            public void Validate()
            {
                //TODO: Faz alguma validação
            }
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

        public class Deletable : Aggregate, IEntityDeletable
        {
            public Deletable(Guid id, string nome, Metodologia metodologia, StatusAvaliacao status)
                 : this(nome, metodologia, status)
            {
                Id = id;
            }

            public Deletable(string nome, Metodologia metodologia, StatusAvaliacao status)
            {
                Nome = nome;
                Metodologia = metodologia;
                Status = status;
            }

            public override bool CadastroCompleto => true;

            public string Nome { get; private set; }

            public Metodologia Metodologia { get; private set; }

            public StatusAvaliacao Status { get; private set; }

            public DateNullable DeletedDate { get; private set; }

            public void Delete()
            {
                DeletedDate = new DateNullable()
                    .SetDateValueWithDateOfNow();
            }
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

            #endregion Properties

            #region Constructors

            public StatusAvaliacao(TipoStatusAvaliacao tipo)
            {
                Tipo = tipo;
            }

            #endregion Constructors

            #region Methods

            public override IEnumerable<object> GetEqualityComponents()
            {
                yield return Tipo;
            }

            #endregion Methods
        }

        public class MongoContext : Context
        {
            public MongoContext(MongoSettings mongoSettings, IMongoClient mongoClient)
                : base(mongoSettings, mongoClient)
            {
            }
        }

        [Trait("Repositories", "Métodos Escrita")]
        [Fact(DisplayName = "Deve inserir pelo repositório no mongodb", Skip = "teste local")]
        public void Deve_Inserir_Pelo_Repositorio()
        {
            //given
            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = CONNECTION_STRING,
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Deletable>(context);

            //when
            mongoRepository.Insert(new Deletable("Nome 3", new Metodologia(TipoMetodologia.NPS), new StatusAvaliacao(TipoStatusAvaliacao.Aguardando)));
            var count = context.SaveChanges();

            //then
            count.Should().Be(1);
        }

        [Trait("Repositories", "Métodos Escrita de coleção")]
        [Fact(DisplayName = "Deve inserir coleção pelo repositório no mongodb")]
        public void Deve_Inserir_Colecao_Pelo_Repositorio()
        {
            //given
            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = CONNECTION_STRING,
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Deletable>(context);

            //when
            List<Deletable> entities = new List<Deletable>
            {
                new Deletable("Nome 3", new Metodologia(TipoMetodologia.NPS), new StatusAvaliacao(TipoStatusAvaliacao.Aguardando)),
                new Deletable("Nome 4", new Metodologia(TipoMetodologia.NPS), new StatusAvaliacao(TipoStatusAvaliacao.Aguardando))
            };

            mongoRepository.InsertRange(entities);
            var count = context.SaveChanges();

            //then
            count.Should().Be(1);
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve buscar todos pelo repositório no mongodb", Skip = "teste local")]
        public void Deve_Obter_Todos_Pelo_Repositorio()
        {
            //given
            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = CONNECTION_STRING,
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Deletable>(context);

            //when
            var all = mongoRepository.GetAll();

            //then
            all.Should().NotBeNull();
            all.Should().NotBeEmpty();
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve buscar pelo repositório no mongodb", Skip = "teste local")]
        public void Deve_Obter_Pelo_Repositorio()
        {
            //given
            var id = Guid.Parse("67fbf9a1-adce-40ba-93b0-a1329e9ce66b");

            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = CONNECTION_STRING,
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Deletable>(context);

            //when
            var aggregate = mongoRepository.GetById(id);

            //then
            aggregate.Should().NotBeNull();
            aggregate.Id.Should().Be(id);
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve buscar todos pelos ids pelo repositório no mongodb", Skip = "teste local")]
        public void Deve_Obter_Todos_Por_Ids_Pelo_Repositorio()
        {
            //given
            var ids = new[]
            {
                Guid.Parse("67fbf9a1-adce-40ba-93b0-a1329e9ce66b"),
                Guid.Parse("de3ce561-6c84-4d2f-ab4f-9d5c74ad5293")
            };

            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = CONNECTION_STRING,
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Deletable>(context);

            //when
            var all = mongoRepository.GetAllByIds(ids);

            //then
            all.Should().NotBeNull();
            all.Should().NotBeEmpty();
            all.Should().HaveCount(ids.Length);
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
                ConnectionString = CONNECTION_STRING,
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

        [Fact(DisplayName = "Deve excluir o usuário", Skip = "Teste Local")]
        public void Deve_Excluir()
        {
            //given
            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = CONNECTION_STRING,
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Deletable>(context);

            var deletable = new Deletable(Guid.Parse("3f32762f-7ed0-484f-90ca-1cc5e6a980c1"), "Nome 3", new Metodologia(TipoMetodologia.NPS), new StatusAvaliacao(TipoStatusAvaliacao.Aguardando));

            //when
            mongoRepository.Delete(deletable);
            var count = context.SaveChanges();

            //then
            count.Should().Be(1);
        }

        [Fact(DisplayName = "Deve excluir uma coleção de usuários", Skip = "Teste Local")]
        public void Deve_Excluir_Colecao_Usuarios()
        {
            //given
            var mongoSettings = new MongoSettings()
            {
                DatabaseName = "dev-sdk-test",
                ConnectionString = CONNECTION_STRING,
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Deletable>(context);

            var deletables = new List<Deletable>
            {
                new Deletable(Guid.Parse("02915140-06d3-4981-8118-c42f4f9b0f69"), "Nome 3", new Metodologia(TipoMetodologia.NPS), new StatusAvaliacao(TipoStatusAvaliacao.Aguardando)),
                new Deletable(Guid.Parse("864fad4d-a6b0-433d-8024-4856274ccb1e"), "Nome 4", new Metodologia(TipoMetodologia.NPS), new StatusAvaliacao(TipoStatusAvaliacao.Aguardando))
            };

            //when
            mongoRepository.DeleteRange(deletables);
            var count = context.SaveChanges();

            //then
            count.Should().Be(2);
        }
    }
}