using FluentAssertions;
using Optsol.Components.Repository.Domain.ValueObjects;
using Xunit;

namespace Optsol.Components.Repository.Test.ValueObjects
{
    public class DateNullableSpec
    {
        [Trait("Value Objects", "Comparar Objetos")]
        [Fact(DisplayName = "Duas DateNullables com os mesmos valores deve retornar verdadeiro")]
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
        [Fact(DisplayName = "Duas DateNullables com os valores diferentes devem retornar false")]
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
    }
}
