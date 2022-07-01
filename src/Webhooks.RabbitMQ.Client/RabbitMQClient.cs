using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Webhooks.RabbitMQ.Client.Interfaces;
using Webhooks.RabbitMQ.Models.Exceptions;

namespace Webhooks.RabbitMQ.Client
{
    public class RabbitMQClient : IRabbitMQClient
    {
        private readonly IConnectionFactory _connectionFactory;

        private IConnection _connection;
        private bool _disposed;

        public RabbitMQClient(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                Thread.Sleep(2000);

                _connection = _connectionFactory.CreateConnection();
            }

            var result = !IsConnected;
            
            if (!IsConnected)
            {
                const string message = "RabbitMQ connections could not be created and opened";
                throw new RabbitMQInvalidException(message);
            }

            return result;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                const string message = "No RabbitMQ connections are available to perform this action.";
                throw new RabbitMQInvalidException(message);
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException exc)
            { 
                throw new RabbitMQInvalidException(exc.Message);
            }
        }
    }
}
