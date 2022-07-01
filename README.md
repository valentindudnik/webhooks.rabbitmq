# Webhooks.RabbitMQ v1.0.1

**Webhooks.RabbitMQ** contains infrastructure for integration and creating new producers and consumers with RabbitMQ.

# Technological Stack:

- NET6 / C#
- RabbitMQ.Client

# Nuget Packages

[Webhooks.RabbitMQ.Client v1.0.1](https://www.nuget.org/packages/Webhooks.RabbitMQ.Client/1.0.1?_src=template)

[Webhooks.RabbitMQ.Models v1.0.1](https://www.nuget.org/packages/Webhooks.RabbitMQ.Models/1.0.1?_src=template)

Configure RabbitMQ:
```
const string rabbitMQSectionName = "RabbitMQ";
var rabbitMQConfiguration = builder.Configuration.GetSection(rabbitMQSectionName).Get<RabbitMQConfiguration>();
builder.Services.ConfigureRabbitMQClient(rabbitMQConfiguration);
```

Configure Producers:
```
using Webhooks.RabbitMQ.Models.Events;

public class CustomEvent: BaseEvent {
  ...
}

public interfaces ICustomProducer {
      void Send(CustomEvent customEvent);
}

public class CustomProducer : RabbitMQProducer, ICustomProducer
{
        public CustomProducer(IRabbitMQClient client, ILogger<RabbitMQProducer> logger) : base(client, logger)
        { }
        
        public void Send(CustomEvent customEvent)
        {
            Publish(QueueNames.CustomQueue, customEvent);
        }
}

builder.Services.AddSingleton<ICustomConsumer, CustomConsumer>();
```

Configure Consumers:
```
public class CustomConsumer : RabbitMQConsumer, ICustomConsumer
{
        public CustomConsumer(IRabbitMQClient client) : base(client)
        { }

        protected override async void ReceivedEvent(object sender, BasicDeliverEventArgs eventArgs)
        {
          ...
        }
}

public interface ICustomConsumer: IRabbitMQConsumer
{ }

app.Lifetime.ConfigureRabbitMQListener(QueueNames.CustomQueue, serviceProvider.GetService<ICustomConsumer>());
```

appsettings.json:

```
  "RabbitMQ": {
    "HostName": "[HostName]",
    "UserName": "[UserName]",
    "Password": "[Password]",
    "VirtualHost": "[VirtualHost]",
    "SslEnabled": "[SslEnabled]"
  },
```

