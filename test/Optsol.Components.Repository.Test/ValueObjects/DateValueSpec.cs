using FluentAssertions;
using Optsol.Components.Repository.Domain.Exceptions;
using Optsol.Components.Repository.Domain.ValueObjects;
using System;
using Xunit;

namespace Optsol.Components.Repository.Test.ValueObjects
{
    public class DateValueSpec
    {
        [Trait("Value Objects", "Comparar Objetos")]
        [Fact(DisplayName = "Duas DateValues com os mesmos valores deve retornar verdadeiro")]
        public void Deve_Comparar_Dois_Objetos_Tipo_Date_Value_Verdadeiro()
        {
            //given
            var objetoDateValueUm = DateValue.Create();
            var objetoDateValueDois = DateValue.Create();

            //when
            var comparacaoEquals = objetoDateValueUm.Equals(objetoDateValueDois);
            var comparacaoOperaror = objetoDateValueUm == objetoDateValueDois;

            //then
            comparacaoEquals.Should().BeTrue("Os valores devem ser iguais");
            comparacaoOperaror.Should().BeTrue("Os valroes devem ser iguais");
        }

        [Trait("Value Objects", "Comparar Objetos")]
        [Fact(DisplayName = "Duas DateValues com os valores diferentes devem retornar false")]
        public void Deve_Comparar_Dois_Objetos_Tipo_Date_Value_False()
        {
            //given
            var objetoDateValueUm = DateValue.Create();
            var objetoDateValueDois = DateValue.Create();

            //when
            objetoDateValueDois.SetDateValueWithDateOfNow();
            var comparacao = objetoDateValueUm != objetoDateValueDois;

            //then
            comparacao.Should().BeTrue("Os valores não devem ser iguais");
        }

        [Trait("Value Objects", "Exceção")]
        [Fact(DisplayName = "Deve lançar exceção quando informar MinValue do DateTime")]
        public void Deve_Lancar_Excecao_Quando_Informar_MinDate_Value()
        {
            //given
            var data = DateTime.MinValue;
            var dateValueValueObject = new DateValue();

            //when
            Action action = () => dateValueValueObject.SetDateValueWithDate(data);

            //then
            action.Should().Throw<DateValueException>();
        }

        [Trait("Value Objects", "Método")]
        [Fact(DisplayName = "Deve exibir o valor da classe formatado como string")]
        public void Deve_Exibir_Valor_Classe_Como_String()
        {
            //given
            var data = new DateTime(2021, 11, 21, 23, 00, 00);
            var dateValueValueObject = new DateValue().SetDateValueWithDate(data);

            //when
            var comoString = dateValueValueObject.ToString();

            //then
            comoString.Should().Be(data.ToShortDateString());
        }

    }
}
