namespace Webhooks.RabbitMQ.Models.Configurations
{
    public class RabbitMQConfiguration
    {
        public string? HostName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? VirtualHost { get; set; }
        public bool SslEnabled { get; set; }
    }
}
