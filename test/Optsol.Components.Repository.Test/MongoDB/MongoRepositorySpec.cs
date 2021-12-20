using FluentAssertions;
using FluentValidation.Results;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Notifications;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;
using Optsol.Components.Repository.Infra.MongoDB.Repositories;
using Optsol.Components.Repository.Infra.MongoDB.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                DatabaseName = "repository-mongo",
                ConnectionString = "mongodb://127.0.0.1:30001",
            };

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoSettings.ConnectionString)
            );

            var mongoClient = new MongoClient(settings);
            var context = new MongoContext(mongoSettings, mongoClient);
            var mongoRepository = new MongoRepository<Teste>(context);

            //when
            mongoRepository.Insert(new Teste("Nome", new Metodologia(TipoMetodologia.NPS), new StatusAvaliacao(TipoStatusAvaliacao.Publicado)));
            var count = context.SaveChanges();

            //then
            mongoRepository.Should().NotBeNull();
            count.Should().Be(1);
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve buscar pelo repositório no mongodb", Skip = "Teste Local")]
        public void Deve_Obter_Pelo_Repositorio()
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
            var mongoRepository = new MongoRepository<Teste>(context);

            //when
            var all = mongoRepository.GetAll();

            //then
            all.Should().NotBeNull();
            all.Should().NotBeEmpty();
        }
    }
}
