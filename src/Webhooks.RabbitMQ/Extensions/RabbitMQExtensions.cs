using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Webhooks.RabbitMQ.Client.Consumers.Interfaces;
using Webhooks.RabbitMQ.Client.Interfaces;
using Webhooks.RabbitMQ.Client.Producers;
using Webhooks.RabbitMQ.Client.Producers.Interfaces;
using Webhooks.RabbitMQ.Models.Configurations;

namespace Webhooks.RabbitMQ.Client.Extensions
{
    public static class RabbitMQExtensions
    {
        public static void ConfigureRabbitMQClient(this IServiceCollection services, RabbitMQConfiguration rabbitMQConfiguration)
        {
            services.AddSingleton<IRabbitMQClient>(configure =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = rabbitMQConfiguration.HostName,
                    RequestedHeartbeat = TimeSpan.FromSeconds(60),
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                    Ssl = new SslOption
                    {
                        Enabled = true,
                        CertificateValidationCallback = delegate { return true; }
                    },
                    VirtualHost = rabbitMQConfiguration.UserName
                };

                if (!string.IsNullOrEmpty(rabbitMQConfiguration.UserName))
                {
                    factory.UserName = rabbitMQConfiguration.UserName;
                }

                if (!string.IsNullOrEmpty(rabbitMQConfiguration.Password))
                {
                    factory.Password = rabbitMQConfiguration.Password;
                }

                return new RabbitMQClient(factory);
            });

            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            services.AddSingleton<INotificationProducer, NotificationProducer>();
        }

        public static void ConfigureRabbitMQListener(this IHostApplicationLifetime hostApplicationLifetime, string queueName, IRabbitMQConsumer consumer)
        {
            hostApplicationLifetime.ApplicationStarted.Register(() => consumer.Consume(queueName));
            hostApplicationLifetime.ApplicationStopping.Register(() => consumer.Disconnect());
        }
    }
}
