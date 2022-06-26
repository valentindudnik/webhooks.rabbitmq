using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Webhooks.RabbitMQ.Client.Consumers.Interfaces;
using Webhooks.RabbitMQ.Client.Interfaces;

namespace Webhooks.RabbitMQ.Client.Consumers
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly IRabbitMQClient _client;

        public RabbitMQConsumer(IRabbitMQClient client)
        {
            _client = client;
        }

        public void Consume(string queueName)
        {
            var channel = _client.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            //Create event when something receive
            consumer.Received += ReceivedEvent!;

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        protected virtual async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            await Task.FromResult(true);
        }

        public void Disconnect()
        {
            _client.Dispose();
        }
    }
}
