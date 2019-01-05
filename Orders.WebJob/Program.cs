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
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(b =>
                {
                    //// Adding command line as a configuration source
                    //b.AddCommandLine(args);
                    b.SetBasePath(Directory.GetCurrentDirectory());
                    b.AddJsonFile("appsettings.json");
                })
                .UseEnvironment("Development")
                .ConfigureWebJobs(b =>
                {
                    b.AddAzureStorageCoreServices()
                     .AddAzureStorage()
                     .AddServiceBus();
                })
                
                .ConfigureLogging((context, b) =>
                {
                    b.SetMinimumLevel(LogLevel.Debug);
                    b.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    // add some sample services to demonstrate job class DI
                    services.AddSingleton<ISampleServiceA, SampleServiceA>();
                    services.AddSingleton<ISampleServiceB, SampleServiceB>();
                })
                .UseConsoleLifetime();
                
            var confbuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //.AddEnvironmentVariables();

            IConfigurationRoot configuration = confbuilder.Build();

            foreach(var env in configuration.GetChildren())
            {
                Console.WriteLine($"{env.Key}::{env.Value}");
            }

            var rabbitMqUri = configuration.GetValue<string>("RabbitMqUri");
            var rabbitMqUriEnv = Environment.GetEnvironmentVariable("RabbitMqUri");

            Console.WriteLine("RabbitMqUri : " + rabbitMqUri);
            Console.WriteLine("RabbitMqUriEnv : " + rabbitMqUriEnv);
            
            var subscriber = new SubscriberRabbitMq(configuration);
            subscriber.SubscribeAndProcessOrdersFromRabbitMq();

            var host = builder.Build();

            host.Run();
        }
    }
}
