using System;

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
