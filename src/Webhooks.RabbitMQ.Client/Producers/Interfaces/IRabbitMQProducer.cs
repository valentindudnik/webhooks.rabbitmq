using Webhooks.RabbitMQ.Models.Events;

namespace Webhooks.RabbitMQ.Client.Producers.Interfaces
{
    public interface IRabbitMQProducer
    {
        void Publish(string queueName, BaseEvent baseEvent);
    }
}
