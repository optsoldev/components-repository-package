using FluentAssertions;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Core;
using Optsol.Components.Repository.Infra.Mock.Entities.Core;
using Optsol.Components.Repository.Infra.Mock.Repositories;
using Optsol.Components.Repository.Test.Repositories;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Optsol.Components.Repository.Test.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitySpec
    {
        public class Entidade : Entity, IEntityDeletable
        {
            public DateNullable DeletedDate { get; protected set; }

            public void Delete()
            {
                DeletedDate = DateNullable.Create().SetDateValueWithDateOfNow();
            }

            public static Entidade Create() => new();
        }

        [Trait("Entities", "Construir Objetos")]
        [Fact(DisplayName = "Deve contruir uma Entity com os dados padrões")]
        public void Deve_Contruir_Entidade_Dados_Padroes()
        {
            //given
            Entidade aggregateRoot;

            //when
            aggregateRoot = Entidade.Create();

            //then
            aggregateRoot.Should().NotBeNull();
            aggregateRoot.Id.Should().NotBeEmpty();
        }

        [Trait("Entities", "Construir Objetos")]
        [Theory(DisplayName = "Deve Inicializar o CreaditCard com CreateDate preenchido com a data atual")]
        [InlineData("5328647840011771", 795, "2023-05-16")]
        public void Deve_Inicializar_Entidade_Preenchido_DataAtual(string number, int code, string validity)
        {
            //given 
            var novaDataValidade = DateValue.Create().SetDateValueWithDate(DateTime.Parse(validity));

            //when
            var novoCartao = CreditCard.Create(number, code, novaDataValidade);

            //then
            novoCartao.CreateDate.Date.Date.Should().Be(DateTime.Now.Date, "As datas deveriam ser iguais");
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar duas Entity pela interface IEntity")]
        public void Deve_Comparar_Duas_Entidades_Pela_Interface()
        {
            //given
            IEntity entityUm = Entidade.Create();
            IEntity entityDois = Entidade.Create();

            //when
            var comparacao = entityUm.Equals(entityDois);

            //then
            comparacao.Should().BeFalse();
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar duas Entity iguais")]
        public void Deve_Comparar_Duas_Entidades_Iguais()
        {
            //given
            var entity = Entidade.Create();
            var entityCopia = entity;

            //when
            var comparacao = entity.Equals(entityCopia);

            //then
            comparacao.Should().BeTrue();
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar duas Entity pela classe")]
        public void Deve_Comparar_Duas_Entidades_Pela_Classe()
        {
            //given
            object entityUm = Entidade.Create();
            object entityDois = Entidade.Create();

            //when
            var comparacao = entityUm.Equals(entityDois);

            //then
            comparacao.Should().BeFalse();
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar uma Entity com um objeto Null")]
        public void Deve_Comparar_Duas_Entidades_Pela_Classe_Com_Objeto_Null()
        {
            //given
            object entityUm = Entidade.Create();
            object entityDois = null;

            //when
            var comparacao = entityUm.Equals(entityDois);

            //then
            comparacao.Should().BeFalse();
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve verificar HashCode único para cada Entity")]
        public void Deve_Verificar_HashCode_Unico_Para_Entity()
        {
            //given
            var entityUm = Entidade.Create();
            var entityDois = Entidade.Create();

            var hashCodeUm = entityUm.GetHashCode();
            var hashCodeDois = entityDois.GetHashCode();

            //when
            var comparacao = hashCodeUm.Equals(hashCodeDois);

            //then
            comparacao.Should().BeFalse();
            hashCodeUm.Should().Be($"{entityUm.GetType()}{entityUm.Id.GetHashCode()}".GetHashCode());
            hashCodeDois.Should().Be($"{entityDois.GetType()}{entityDois.Id.GetHashCode()}".GetHashCode());
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar dois Aggregates do mesmo tipo e Id e retornar verdadeiro")]
        public void Deve_Comparar_Dois_Objetos_Tipo_Aggregate_Verdadeiro()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();

            var customerUm = readRepository.GetAll().First();
            var customerDois = readRepository.GetById(customerUm.Id);

            //when
            var comparacaoEquals = customerUm.Person.Equals(customerDois.Person);
            var comparacaoOperaror = customerUm.Person == customerDois.Person;

            //then
            comparacaoEquals.Should().BeTrue("Os valores devem ser iguais");
            comparacaoOperaror.Should().BeTrue("Os valroes devem ser iguais");
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar dois Aggregates diferentes e retornar falso")]
        public void Deve_Comparar_Dois_Objetos_Tipo_Aggregate_Falso()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();

            var customerUm = readRepository.GetAll().First();
            var customerDois = MockRepositorySpec.CreateCustomer();

            //when
            var comparacaoEquals = !customerUm.Person.Equals(customerDois.Person);
            var comparacaoOperaror = customerUm.Person != customerDois.Person;

            //then
            comparacaoEquals.Should().BeTrue("Os valores não devem ser iguais");
            comparacaoOperaror.Should().BeTrue("Os valroes não devem ser iguais");
        }

        [Trait("Entities", "Métodos")]
        [Fact(DisplayName = "Deve excluir uma Entity logicamente implementando interface IEntity")]
        public void Deve_Excluir_Entidade_Logicamente()
        {
            //given
            IEntityDeletable entidade = Entidade.Create();
            
            //when
            entidade.Delete();

            //then
            entidade.Should().NotBeNull();
            entidade.IsDeleted().Should().BeTrue();
            entidade.DeletedDate.Should().NotBeNull();
        }
    }
}
