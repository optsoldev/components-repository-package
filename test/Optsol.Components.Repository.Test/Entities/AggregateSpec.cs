using FluentAssertions;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.ValueObjects;
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
    public class AggregateRootSpec
    {
        public class Aggregate : AggregateRoot
        {
            public static Aggregate Create() => new();
        }

        [Trait("Entities", "Construir Objetos")]
        [Fact(DisplayName = "Deve contruir um AggregateRoot com os dados padrões")]
        public void DeveContruirUmAggregateRootDadosPadroes()
        {
            //given
            AggregateRoot aggregateRoot;

            //when
            aggregateRoot = Aggregate.Create();

            //then
            aggregateRoot.Should().NotBeNull();
            aggregateRoot.CreateDate.Should().NotBeNull();
            aggregateRoot.Id.Should().NotBeNull();
        }

        [Trait("Entities", "Construir Objetos")]
        [Theory(DisplayName = "Deve Inicializar o Customer com CreateDate preenchido com a data atual")]
        [InlineData("Guilherme", "Rodrigues", "guilherme.conrado@optsol.com.br", "1984-01-01")]
        public void DeveInicializarEntidadePreenchidoDataAtual(string nome, string sobreNome, string email, string dataNascimento)
        {
            //given 
            var novaPessoa = Person.Create(nome, sobreNome);
            var novoEmail = Email.Create(email);
            var novaDataNascimento = DateValue.Create().SetDateValueWithDate(DateTime.Parse(dataNascimento));

            //when
            var customerObject = Customer.Create(novaPessoa, novoEmail, novaDataNascimento);

            //then
            customerObject.CreateDate.Date.Date.Should().Be(DateTime.Now.Date, "As datas deveriam ser iguais");
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar dois AggregateRoot")]
        public void DeveCompararDoisAggregateRoot()
        {
            //given
            var aggregateRootUm = Aggregate.Create();
            var aggregateRootDois = Aggregate.Create();

            //when
            var comparacao = aggregateRootUm.Equals(aggregateRootDois);

            //then
            comparacao.Should().BeFalse();
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve verificar HashCode único para cada AggregateRoot")]
        public void DeveVerificarHashCodeUnicoParaAggregateRoot()
        {
            //given
            IAggregateRoot aggregateRootUm = Aggregate.Create();
            IAggregateRoot aggregateRootDois = Aggregate.Create();
            
            var hashCodeUm = aggregateRootUm.GetHashCode();
            var hashCodeDois = aggregateRootDois.GetHashCode();

            //when
            var comparacao = hashCodeUm.Equals(hashCodeDois);

            //then
            comparacao.Should().BeFalse();
            hashCodeUm.Should().Be($"{aggregateRootUm.GetType()}{aggregateRootUm.Id.GetHashCode()}".GetHashCode());
            hashCodeDois.Should().Be($"{aggregateRootDois.GetType()}{aggregateRootDois.Id.GetHashCode()}".GetHashCode());
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar dois Aggregates do mesmo tipo e Id e retornar verdadeiro")]
        public void DeveCompararDoisObjetosTipoAggregateVerdadeiro()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();

            var customerUm = readRepository.GetAll().First();
            var customerDois = readRepository.GetByKey(customerUm.Id);

            //when
            var comparacaoEquals = customerUm.Equals(customerDois);
            var comparacaoOperaror = customerUm == customerDois;

            //then
            comparacaoEquals.Should().BeTrue("Os valores devem ser iguais");
            comparacaoOperaror.Should().BeTrue("Os valroes devem ser iguais");
        }

        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar dois Aggregates diferentes e retornar falso")]
        public void DeveCompararDoisObjetosTipoAggregateFalso()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();

            var customerUm = readRepository.GetAll().First();
            var customerDois = MockRepositorySpec.CreateCustomer();

            //when
            var comparacaoEquals = !customerUm.Equals(customerDois);
            var comparacaoOperaror = customerUm != customerDois;

            //then
            comparacaoEquals.Should().BeTrue("Os valores não devem ser iguais");
            comparacaoOperaror.Should().BeTrue("Os valroes não devem ser iguais");
        }
    }
}
