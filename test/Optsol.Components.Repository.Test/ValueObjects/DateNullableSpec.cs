using FluentAssertions;
using Optsol.Components.Repository.Domain.ValueObjects;
using System;
using Xunit;

namespace Optsol.Components.Repository.Test.ValueObjects
{
    public class DateNullableSpec
    {
        [Trait("Value Objects", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comparar duas DateNullables com os mesmos valores deve retornar verdadeiro")]
        public void DeveCompararDoisObjetosTipoDateNullableVerdadeiro()
        {
            //given
            var objetoDateNullableUm = DateNullable.Create();
            var objetoDateNullableDois = DateNullable.Create();

            //when
            var comparacaoEquals = objetoDateNullableUm.Equals(objetoDateNullableDois);
            var comparacaoOperaror = objetoDateNullableUm == objetoDateNullableDois;

            //then
            comparacaoEquals.Should().BeTrue("Os valores devem ser iguais");
            comparacaoOperaror.Should().BeTrue("Os valroes devem ser iguais");
        }

        [Trait("Value Objects", "Comparar Objetos")]
        [Fact(DisplayName = "Deve comaprar duas DateNullables com os valores diferentes devem retornar false")]
        public void DeveCompararDoisObjetosTipoDateNullableFalse()
        {
            //given
            var objetoDateNullableUm = DateNullable.Create();
            var objetoDateNullableDois = DateNullable.Create();

            //when
            objetoDateNullableDois.SetDateValueWithDateOfNow();
            var comparacao = objetoDateNullableUm != objetoDateNullableDois;

            //then
            comparacao.Should().BeTrue("Os valores não devem ser iguais");
        }

        [Trait("Value Objects", "Método")]
        [Fact(DisplayName = "Deve exibir o valor da classe formatado como string")]
        public void DeveExibirValorClasseComoString()
        {
            //given
            var data = new DateTime(2021, 11, 21, 23, 00, 00);
            var dateValueNullableValueObject = new DateValue().SetDateValueWithDate(data);

            //when
            var comoString = dateValueNullableValueObject.ToString();

            //then
            comoString.Should().Be(data.ToShortDateString());
        }
    }
}
