namespace Webhooks.RabbitMQ.Client.Consumers.Interfaces
{
    public interface IRabbitMQConsumer
    {
        void Consume(string queueName);
        void Disconnect();
    }
}
