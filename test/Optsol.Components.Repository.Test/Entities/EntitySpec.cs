using FluentAssertions;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Core;
using System;
using Xunit;

namespace Optsol.Components.Repository.Test.Entities
{
    public class EntitySpec
    {
        [Trait("Entities", "Construir Objetos")]
        [Theory(DisplayName = "Deve Inicializar o CreaditCard com CreateDate preenchido com a data atual")]
        [InlineData("5328647840011771", 795, "16-05-2023")]
        public void DeveInicializarEntidadePreenchidoDataAtual(string number, int code, string validity)
        {
            //given 
            var novaDataValidade = DateValue.Create().SetDateValueWithDate(DateTime.Parse(validity));

            //when
            var novoCartao = CreaditCard.Create(number, code, novaDataValidade);

            //then
            novoCartao.CreateDate.Date.Date.Should().Be(DateTime.Now.Date, "As datas deveriam ser iguais");
        }
    }
}
