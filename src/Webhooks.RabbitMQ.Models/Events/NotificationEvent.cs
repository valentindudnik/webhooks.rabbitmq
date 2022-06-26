namespace Webhooks.RabbitMQ.Models.Events
{
    public class NotificationEvent : BaseEvent
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
    }
}
