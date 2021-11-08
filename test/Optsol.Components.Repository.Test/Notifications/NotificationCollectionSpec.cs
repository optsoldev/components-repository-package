﻿using FluentAssertions;
using Optsol.Components.Repository.Domain.Notifications;
using Xunit;

namespace Optsol.Components.Repository.Test.Notifications
{
    public class NotificationCollectionSpec
    {
        [Trait("Notifications", "Construir Objetos")]
        [Fact(DisplayName = "Deve construir um NotificationCollection com os dados padrões")]
        public void DeveConstruirUmNotificationCollectionComDadosPadroes()
        {
            //given
            string key = "Chave";
            string message = "Mensagem";

            var notification = Notification.Create(key, message);

            NotificationCollection notificationCollection;

            //when
            notificationCollection = NotificationCollection.Create();

            //then
            notificationCollection.Should().NotBeNull();
        }

        [Trait("Notifications", "Métodos")]
        [Fact(DisplayName = "Deve limpar todos os registros da lista de notificações")]
        public void DeveLimparOsRegistrosDaListaDeNotificacoes()
        {
            //given
            string key = "Chave";
            string message = "Mensage";

            var notificationCollection = NotificationCollection.Create();

            notificationCollection.Insert(Notification.Create(key, message));
            notificationCollection.Insert(Notification.Create(key, message));

            //when
            notificationCollection.Clear();

            //then
            notificationCollection.Should().NotBeNull();
            notificationCollection.HasNotifications.Should().BeFalse();
            notificationCollection.GetNotifications().Should().BeEmpty();
        }

        [Trait("Notifications", "Métodos")]
        [Fact(DisplayName = "Deve buscar notificação pela chave")]
        public void DeveBuscarNotificacaoPelaChave()
        {
            //given
            string key = "Chave";
            string message = "Mensage";

            var notificationCollection = NotificationCollection.Create();

            notificationCollection.Insert(Notification.Create(key, message));

            //when
            var notification = notificationCollection.FindByKey(key);

            //then
            notification.Should().NotBeNull();
            notification.Key.Should().Be(key);
            notification.Message.Should().Be(message);
        }

        [Trait("Notifications", "Métodos")]
        [Fact(DisplayName = "Deve buscar notificação por chaves")]
        public void DeveBuscarNotificacaoPorChaves()
        {
            //given
            string key = "Chave";
            string message = "Mensage";

            var notificationCollection = NotificationCollection.Create();

            notificationCollection.Insert(Notification.Create(key, message));
            notificationCollection.Insert(Notification.Create(key, message));

            //when
            var notifications = notificationCollection.FindByKeys(key);

            //then
            notifications.Should().NotBeNull();
            notifications.Should().NotBeEmpty();
            notifications.Should().HaveCount(2);
        }

        [Trait("Notifications", "Métodos")]
        [Fact(DisplayName = "Deve remover registro da lista de notificações")]
        public void DeveRemoverRegistroDaListaDeNotificacoes()
        {
            //given
            string key = "Chave";
            string message = "Mensage";

            var notificationCollection = NotificationCollection.Create();

            var notification = Notification.Create(key, message);

            notificationCollection.Insert(notification, Notification.Create(key, message));

            //when
            notificationCollection.Delete(notification);

            //then
            notificationCollection.Should().NotBeNull();
            notificationCollection.HasNotifications.Should().BeTrue();
            notificationCollection.GetNotifications().Should().NotBeEmpty();
            notificationCollection.Total.Should().Be(1);
        }
    }
}
