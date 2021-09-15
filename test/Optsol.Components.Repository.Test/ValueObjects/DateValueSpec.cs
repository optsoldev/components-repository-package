using FluentAssertions;
using Optsol.Components.Repository.Domain.ValueObjects;
using System;
using Xunit;

namespace Optsol.Components.Repository.Test.ValueObjects
{
    public class DateValueSpec
    {
        [Trait("Value Objects", "Comparar Objetos")]
        [Fact(DisplayName = "Duas DateValues com os mesmos valores deve retornar verdadeiro")]
        public void DeveCompararDoisObjetosTipoDateValueVerdadeiro()
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
        public void DeveCompararDoisObjetosTipoDateValueFalse()
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
    }
}
