using FluentAssertions;
using Optsol.Components.Repository.Domain.ValueObjects;
using Xunit;

namespace Optsol.Components.Repository.Test
{
    public class KeySpec
    {
        [Trait("Object Key", "Comparar Objetos")]
        [Fact(DisplayName = "Duas Keys com os mesmo Id deve retornar verdadeiro")]
        public void DeveCompararDoisObjetosTipoKeyVerdadeiro()
        {
            //given
            var objetoKeyUm = Key.Create(1);
            var objetoKeyDois = Key.Create(1);

            //when
            var comparacaoEquals = objetoKeyUm.Equals(objetoKeyDois);
            var comparacaoOperaror = objetoKeyUm == objetoKeyDois;

            //then
            comparacaoEquals.Should().BeTrue("Ids devem ser iguais");
            comparacaoOperaror.Should().BeTrue("Ids devem ser iguais");
        }

        [Trait("Object Key", "Comparar Objetos")]
        [Fact(DisplayName = "Duas Keys com os diferentes Id deve retornar false")]
        public void DeveCompararDoisObjetosTipoKeyFalse()
        {
            //given
            var objetoKeyUm = Key.Create(1);
            var objetoKeyDois = Key.Create(2);

            //when
            var comparacao = objetoKeyUm != objetoKeyDois;

            //then
            comparacao.Should().BeTrue("Ids não devem ser iguais");
        }

    }
}
