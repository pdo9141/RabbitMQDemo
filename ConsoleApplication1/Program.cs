using System;
using System.Text;
using RabbitMQ.Client;

namespace ConsoleApplication1
{
    class Program
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module1.Sample4";
        private const string ExchangeName = "";

        static void Main(string[] args)
        {
            //CreateExchangeAndQueues();
            //BasicSendDemo();
            PersistenceDemo();
        }

        static void CreateExchangeAndQueues()
        {
            Console.WriteLine("Starting RabbitMQ Queue Creator");
            Console.WriteLine();
            Console.WriteLine();

            var connectionFactory = new ConnectionFactory { HostName = HostName, UserName = UserName, Password = Password };
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();

            model.QueueDeclare("PHDQueue", true, false, false, null);
            Console.WriteLine("PHDQueue queue created");

            model.ExchangeDeclare("PHDExchange", ExchangeType.Topic);
            Console.WriteLine("PHDExchange exchange created");

            model.QueueBind("PHDQueue", "PHDExchange", "cars");
            Console.WriteLine("PHDQueue and PHDExchange bound");

            Console.WriteLine("Ending RabbitMQ Queue Creator");
        }

        static void BasicSendDemo()
        {
            Console.WriteLine("Starting Basic Demo");
            Console.WriteLine();
            Console.WriteLine();

            var connectionFactory = new ConnectionFactory { HostName = HostName, UserName = UserName, Password = Password };
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();

            var properties = model.CreateBasicProperties();
            properties.Persistent = false;

            //Serialize
            byte[] messageBuffer = Encoding.Default.GetBytes("this is my message");

            //Send message
            model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);

            Console.WriteLine("Message sent to Module1.Sample3");
            Console.WriteLine("Ending Basic Demo");
        }

        static void PersistenceDemo()
        {
            Console.WriteLine("Starting Persistence Demo");
            Console.WriteLine();
            Console.WriteLine();

            var connectionFactory = new ConnectionFactory { HostName = HostName, UserName = UserName, Password = Password };
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();

            var properties = model.CreateBasicProperties();
            properties.Persistent = true;

            //Serialize
            byte[] messageBuffer = Encoding.Default.GetBytes("this is my persistence message");

            //Send message
            model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);

            Console.WriteLine("Message sent");
            Console.WriteLine("Ending Persistence Demo");
        }
    }
}
