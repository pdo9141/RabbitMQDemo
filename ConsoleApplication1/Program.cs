using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ConsoleApplication1
{
    class Program
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";

        static void Main(string[] args)
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

            Console.ReadLine();
        }
    }
}
