﻿using FluentAssertions;
using Optsol.Components.Repository.Domain.ValueObjects;
using Xunit;

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
    }
}