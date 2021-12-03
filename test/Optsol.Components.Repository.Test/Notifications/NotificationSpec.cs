using FluentAssertions;
using Optsol.Components.Repository.Domain.Exceptions;
using Optsol.Components.Repository.Domain.Notifications;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Optsol.Components.Repository.Test.Notifications
{
    [ExcludeFromCodeCoverage]
    public class NotificationSpec
    {
        [Trait("Notifications", "Construir Objetos")]
        [Fact(DisplayName = "Deve construir um Notification com os dados padrões")]
        public void Deve_Construir_Notification_DadosPadroes()
        {
            //given
            string key = "Chave";
            string message = "Valor";

            Notification notification;

            //when
            notification = Notification.Create(key, message);

            //then
            notification.Should().NotBeNull();
            notification.Key.Should().Be(key);
            notification.Message.Should().Be(message);
        }

        [Trait("Notifications", "Exceção")]
        [Theory(DisplayName = "Deve lançar exceção quando informar MinValue do DateTime")]
        [InlineData("chave", "")]
        [InlineData("", "mensagem")]
        public void Deve_Lancar_Excecao_Quando_Informar_MinDateValue(string key, string message)
        {
            //given
            string keyValue = key;
            string messageValue = message;

            //when
            Action action = () => Notification.Create(key, message);

            //then
            action.Should().Throw<NotificationException>();
        }
    }
}
