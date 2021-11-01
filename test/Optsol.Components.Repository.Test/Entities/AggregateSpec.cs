using FluentAssertions;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Entities.Core;
using System;
using Xunit;

namespace Optsol.Components.Repository.Test.Entities
{
    public class AggregateSpec
    {
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
    }
}
