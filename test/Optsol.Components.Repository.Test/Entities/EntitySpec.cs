using FluentAssertions;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Core;
using Optsol.Components.Repository.Infra.Mock.Entities.Core;
using Optsol.Components.Repository.Infra.Mock.Repositories;
using Optsol.Components.Repository.Test.Repositories;
using System;
using System.Linq;
using Xunit;

namespace Optsol.Components.Repository.Test.Entities
{
    public class EntitySpec
    {
        [Trait("Entities", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar dois Aggregates do mesmo tipo e Id e retornar verdadeiro")]
        public void DeveCompararDoisObjetosTipoAggregateVerdadeiro()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();
            var customerUm = readRepository.GetAll().First();
            var customerDois = readRepository.GetByKey(customerUm.Id);

            //when
            var comparacaoEquals = customerUm.Person.Equals(customerDois.Person);
            var comparacaoOperaror = customerUm.Person == customerDois.Person;

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
            var customerDois = MockReadRepositorySpec.CreateCustomer();

            //when
            var comparacaoEquals = !customerUm.Person.Equals(customerDois.Person);
            var comparacaoOperaror = customerUm.Person != customerDois.Person;

            //then
            comparacaoEquals.Should().BeTrue("Os valores não devem ser iguais");
            comparacaoOperaror.Should().BeTrue("Os valroes não devem ser iguais");
        }

        [Trait("Entities", "Construir Objetos")]
        [Theory(DisplayName = "Deve Inicializar o CreaditCard com CreateDate preenchido com a data atual")]
        [InlineData("5328647840011771", 795, "2023-05-16")]
        public void DeveInicializarEntidadePreenchidoDataAtual(string number, int code, string validity)
        {
            //given 
            var novaDataValidade = DateValue.Create().SetDateValueWithDate(DateTime.Parse(validity));

            //when
            var novoCartao = CreditCard.Create(number, code, novaDataValidade);

            //then
            novoCartao.CreateDate.Date.Date.Should().Be(DateTime.Now.Date, "As datas deveriam ser iguais");
        }
    }
}
