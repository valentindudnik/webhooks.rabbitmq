namespace Webhooks.RabbitMQ.Models.Events
{
    public class BaseEvent
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
    }
}
