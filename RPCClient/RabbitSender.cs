using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RPCClient
{
    public class RabbitSender : IDisposable
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module2.Sample7.Queue";
        private const bool IsDurable = false;

        private const string VirtualHost = "";
        private int Port = 0;

        private string _responseQueue;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private QueueingBasicConsumer _consumer;

        public RabbitSender()
        {
            DisplaySettings();
            SetupRabbitMq();
        }

        private void DisplaySettings()
        {
            Console.WriteLine("Host: {0}", HostName);
            Console.WriteLine("Username: {0}", UserName);
            Console.WriteLine("Password: {0}", Password);
            Console.WriteLine("QueueName: {0}", QueueName);            
            Console.WriteLine("VirtualHost: {0}", VirtualHost);
            Console.WriteLine("Port: {0}", Port);
            Console.WriteLine("IsDurable: {0}", IsDurable);
        }

        private void SetupRabbitMq()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            if (string.IsNullOrEmpty(VirtualHost) == false)
                _connectionFactory.VirtualHost = VirtualHost;
            if (Port > 0)
                _connectionFactory.Port = Port;

            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();

            //Create dynamic response queue
            _responseQueue = _model.QueueDeclare().QueueName;
            _consumer = new QueueingBasicConsumer(_model);
            _model.BasicConsume(_responseQueue, true, _consumer);
        }

        public string Send(string message, TimeSpan timeout)
        {
            var correlationToken = Guid.NewGuid().ToString();

            var properties = _model.CreateBasicProperties();
            properties.ReplyTo = _responseQueue;
            properties.CorrelationId = correlationToken;

            byte[] messageBuffer = Encoding.Default.GetBytes(message);

            var timeoutAt = DateTime.Now + timeout;
            _model.BasicPublish("", QueueName, properties, messageBuffer);

            //Wait for response
            while (DateTime.Now <= timeoutAt)
            {
                var deliveryArgs = (BasicDeliverEventArgs)_consumer.Queue.Dequeue();
                if (deliveryArgs.BasicProperties != null
                    && deliveryArgs.BasicProperties.CorrelationId == correlationToken)
                {
                    var response = Encoding.Default.GetString(deliveryArgs.Body);
                    return response;
                }
            }

            throw new TimeoutException(@"The response was not returned before the timeout specification.");
        }

        public void Dispose()
        {
            if (_model != null)
                _model.Dispose();
            if (_connection != null)
                _connection.Dispose();
        }
    }
}
