using Microsoft.Extensions.Logging;
using Webhooks.RabbitMQ.Client.Interfaces;
using Webhooks.RabbitMQ.Client.Producers.Interfaces;
using Webhooks.RabbitMQ.Models.Common;
using Webhooks.RabbitMQ.Models.Events;

namespace Webhooks.RabbitMQ.Client.Producers
{
    public class NotificationProducer : RabbitMQProducer, INotificationProducer
    {
        public NotificationProducer(IRabbitMQClient client, ILogger<NotificationProducer> logger) : base(client, logger)
        { }

        public void Notify(string title, string message)
        {
            Publish(QueueNames.NotificationsQueue, new NotificationEvent { 
                Id = Guid.NewGuid(),
                Title = title,
                Message = message,
                Created = DateTime.UtcNow
            });
        }
    }
}