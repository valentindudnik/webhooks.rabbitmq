# Webhooks.RabbitMQ v1.0.0

**Webhooks.RabbitMQ** contains infrastructure for integration and creating new producers and consumers with RabbitMQ.

# Nuget Packages

[Webhooks.RabbitMQ.Client v1.0.0](https://www.nuget.org/packages/Webhooks.RabbitMQ.Client/1.0.0?_src=template)

[Webhooks.RabbitMQ.Models v1.0.0](https://www.nuget.org/packages/Webhooks.RabbitMQ.Models/1.0.0?_src=template)

Configure RabbitMQ:
```
const string rabbitMQSectionName = "RabbitMQ";
var rabbitMQConfiguration = builder.Configuration.GetSection(rabbitMQSectionName).Get<RabbitMQConfiguration>();
builder.Services.ConfigureRabbitMQClient(rabbitMQConfiguration);
```

Configure Producers:
```
public class CustomProducer : RabbitMQProducer, ICustomProducer
{
        public CustomProducer(IRabbitMQClient client, ILogger<RabbitMQProducer> logger) : base(client, logger)
        { }
        
        ...
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
    "Password": "[Password]"
  },
```
