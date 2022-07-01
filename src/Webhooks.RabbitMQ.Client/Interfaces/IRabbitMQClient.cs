using RabbitMQ.Client;

namespace Webhooks.RabbitMQ.Client.Interfaces
{
    public interface IRabbitMQClient : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
