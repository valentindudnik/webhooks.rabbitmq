using System.Runtime.Serialization;

namespace Webhooks.RabbitMQ.Models.Exceptions
{
    [Serializable]
    public class RabbitMQInvalidException : ApplicationException
    {
        public RabbitMQInvalidException() 
        { }

        public RabbitMQInvalidException(string message) : base(message)
        { }

        public RabbitMQInvalidException(string message, Exception inner) : base(message, inner)
        { }

        public RabbitMQInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
