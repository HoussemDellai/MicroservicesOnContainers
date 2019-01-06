using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Orders.WebJob
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var rabbitMqUri = Environment.GetEnvironmentVariable("RabbitMqUri");
            
            if (string.IsNullOrEmpty(rabbitMqUri))
            {
                rabbitMqUri = "amqp://guest:guest@localhost:5672//";// for local development
            }
            
            Console.WriteLine("RabbitMqUri : " + rabbitMqUri);

            var subscriber = new SubscriberRabbitMq(rabbitMqUri);
            subscriber.SubscribeAndProcessOrdersFromRabbitMq();
        }
    }
}
