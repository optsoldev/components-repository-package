using FluentAssertions;
using Optsol.Components.Repository.Domain.ValueObjects;
using static Optsol.Components.Repository.Domain.ValueObjects.KeyExtensions;
using System;
using Xunit;
using Optsol.Components.Repository.Domain.Exceptions;

namespace Optsol.Components.Repository.Test.ValueObjects
{
    public class KeySpec
    {
        [Trait("Value Objects", "Comparar Objetos")]
        [Fact(DisplayName = "Duas Keys com os mesmos Ids deve retornar verdadeiro")]
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

        [Trait("Value Objects", "Comparar Objetos")]
        [Fact(DisplayName = "Duas Keys com diferentes Ids deve retornar false")]
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

        [Trait("Value Objects", "Comparar Objetos")]
        [Fact(DisplayName = "Duas Keys nulas deve retornar false")]
        public void DeveCompararDoisObjetosTipoKeyNuloFalse()
        {
            //given
            Key objetoKeyUm = null;
            Key objetoKeyDois = null;

            //when
            var comparacao = objetoKeyUm == objetoKeyDois;

            //then
            comparacao.Should().BeFalse("Ids não devem ser iguais");
        }

        [Trait("Value Objects", "Exceção")]
        [Fact(DisplayName = "Deve lançar exceção quando informar MinValue do DateTime")]
        public void DeveLancarExcecaoQuandoConverter()
        {
            //given
            int id = 1;
            var keyValueObject = Key.Create(id);

            //when
            Action action = () => keyValueObject.AsGuid();

            //then
            action.Should().Throw<KeyException>();
        }

        [Trait("Value Objects", "Exceção")]
        [Fact(DisplayName = "Deve lançar exceção quando informar key como nulo")]
        public void DeveLancarExcecaoQuandoConverterKeyNula()
        {
            //given
            Key keyValueObject = null;

            //when
            Action action = () => keyValueObject.AsGuid();

            //then
            action.Should().Throw<KeyException>();
        }
    },
}
