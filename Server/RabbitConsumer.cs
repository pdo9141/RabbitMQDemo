﻿using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace Server
{
    public class RabbitConsumer : IDisposable
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        //private const string QueueName = "Module2.Sample2";
        private const string QueueName = "Module2.Sample3.Queue1";
        private const string ExchangeName = "";
        private const bool IsDurable = true;

        private const string VirtualHost = "";
        private int Port = 0;

        public delegate void OnReceiveMessage(string message);

        public bool Enabled { get; set; }

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private Subscription _subscription;

        public RabbitConsumer()
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
            Console.WriteLine("ExchangeName: {0}", ExchangeName);
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
            _model.BasicQos(0, 1, false);
        }

        public void Start()
        {
            /*
            var consumer = new QueueingBasicConsumer(_model);
            _model.BasicConsume(QueueName, false, consumer);

            while (Enabled)
            {
                var deliveryArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                var message = Encoding.Default.GetString(deliveryArgs.Body);

                Console.WriteLine("Message Received - {0}", message);
                _model.BasicAck(deliveryArgs.DeliveryTag, false);
            }
            */

            _subscription = new Subscription(_model, QueueName, false);
            var consumer = new ConsumeDelegate(Poll);
            consumer.Invoke();
        }

        private delegate void ConsumeDelegate();

        private void Poll()
        {
            while (Enabled)
            {
                var deliverArgs = _subscription.Next();
                var message = Encoding.Default.GetString(deliverArgs.Body);
                Console.WriteLine("Message Received - {0}", message);
                _subscription.Ack(deliverArgs);
            }
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
