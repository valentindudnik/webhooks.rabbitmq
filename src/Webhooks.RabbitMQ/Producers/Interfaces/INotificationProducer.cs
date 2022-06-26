namespace Webhooks.RabbitMQ.Client.Producers.Interfaces
{
    public interface INotificationProducer : IRabbitMQProducer
    {
        void Notify(string title, string message);
    }
}
