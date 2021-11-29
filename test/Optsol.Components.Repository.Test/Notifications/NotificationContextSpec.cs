using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Optsol.Components.Repository.Domain.Notifications;
using System.Collections.Generic;
using Xunit;

namespace Optsol.Components.Repository.Test.Notifications
{
    public class NotificationContextSpec
    {
        [Trait("Notifications", "Construir Objetos")]
        [Fact(DisplayName = "Deve contruir um NotificationContext com os dados padrões")]
        public void Deve_Contruir_AggregateRoot_DadosPadroes()
        {
            //given
            NotificationContext notificationContext;

            //when
            notificationContext = NotificationContext.Create();

            //then
            notificationContext.Should().NotBeNull();
            notificationContext.Notifications.Should().NotBeNull();
        }

        [Trait("Notifications", "Métodos")]
        [Fact(DisplayName = "Deve adicionar uma notificação")]
        public void Deve_Adicionar_Notificacao_Contexto()
        {
            //given
            var notification = Notification.Create("Chave", "Mensagem");
            var notificationContext = NotificationContext.Create();

            //when
            notificationContext.AddNotification(notification);

            //then
            notificationContext.Should().NotBeNull();
            notificationContext.Notifications.Should().NotBeNull();
            notificationContext.Notifications.HasNotifications.Should().BeTrue();
            notificationContext.Notifications.GetNotifications().Should().NotBeEmpty();
            notificationContext.Notifications.Total.Should().Be(1);
        }


        [Trait("Notifications", "Métodos")]
        [Fact(DisplayName = "Deve adicionar uma notificação")]
        public void Deve_Adicionar_Notificacoes_Contexto()
        {
            //given
            var validationResult = new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("prop", "erro", "x") { ErrorCode = "codigo" } });

            var notification = Notification.Create("Chave", "Mensagem");
            var notificationList = new List<Notification>() { Notification.Create("Chave", "Mensagem") };
            var notificationArray = new Notification[] { Notification.Create("Chave", "Mensagem") };

            var notificationContext = NotificationContext.Create();

            //when
            notificationContext.AddNotification("Chave", "Valor");
            notificationContext.AddNotifications(notification, Notification.Create("Codigo", "Valor"));
            notificationContext.AddNotifications(validationResult);
            notificationContext.AddNotifications(notificationList);
            notificationContext.AddNotifications(notificationArray);
            notificationContext.AddNotifications((ICollection<Notification>)notificationList);

            //then
            notificationContext.Should().NotBeNull();
            notificationContext.Notifications.Should().NotBeNull();
            notificationContext.Notifications.HasNotifications.Should().BeTrue();
            notificationContext.Notifications.GetNotifications().Should().NotBeEmpty();
            notificationContext.Notifications.Total.Should().Be(7);
        }

        [Trait("Notifications", "Métodos")]
        [Fact(DisplayName = "Deve limpar todas as notificações no contexto")]
        public void Deve_Limpar_Notificacoes_Contexto()
        {
            //given
            var notification = Notification.Create("Chave", "Mensagem");
            var notificationContext = NotificationContext.Create();
            notificationContext.AddNotifications(notification, notification);

            //when
            notificationContext.ClearNotifications();

            //then
            notificationContext.Should().NotBeNull();
            notificationContext.Notifications.Should().NotBeNull();
            notificationContext.Notifications.HasNotifications.Should().BeFalse();
            notificationContext.Notifications.Total.Should().Be(0);
            notificationContext.Notifications.GetNotifications().Should().BeEmpty();
        }
    }
}
