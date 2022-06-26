using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Webhooks.RabbitMQ.Client.Interfaces;
using Webhooks.RabbitMQ.Client.Producers.Interfaces;
using Webhooks.RabbitMQ.Models.Events;

namespace Webhooks.RabbitMQ.Client.Producers
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IRabbitMQClient _client;
        private readonly ILogger<RabbitMQProducer> _logger;

        public RabbitMQProducer(IRabbitMQClient client, ILogger<RabbitMQProducer> logger)
        {
            _client = client;
            _logger = logger;
        }

        public virtual void Publish(string queueName, BaseEvent baseEvent)
        {
            const string information = "Info: Sent to RabbitMQ.";

            using (var channel = _client.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(baseEvent);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: string.Empty, routingKey: queueName, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) => _logger.LogInformation(information);
                channel.ConfirmSelect();
            }
        }
    }
}
