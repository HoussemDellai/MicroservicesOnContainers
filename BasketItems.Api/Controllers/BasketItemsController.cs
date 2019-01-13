using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Basket.Api.Models;
using Orders.Api.Models;
using Microsoft.Azure.ServiceBus;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        string ServiceBusConnectionString = "Endpoint=sb://microservicesoncontainers.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=pydtJlzPcGSoHLuO/YcUi6MDZ/liZRI7BTMiwP4glXA=";
        string QueueName = "ordersqueue";

        private readonly BasketContext _context;
        private readonly IConfiguration _configuration;
        private readonly RedisCacheClient _redisCacheClient;

        public BasketItemsController(BasketContext context, IConfiguration configuration, RedisCacheClient redisCacheClient)
        {
            _context = context;
            _configuration = configuration;
            _redisCacheClient = redisCacheClient;
        }

        // POST: api/BasketItems/checkout
        [Route("checkout")]
        [HttpPost]
        public IActionResult Checkout([FromBody] List<BasketItem> basketItems)
        {
            Console.WriteLine("Started Checkout ...");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = CreateOrder(basketItems);

            PublishOrderUsingRabbitMq(order);

            //await PublishOrderUsingAzureServiceBus(order);

            return Ok();
        }

        private void PublishOrderUsingRabbitMq(Order order)
        {
            Console.WriteLine("Started PublishOrderUsingRabbitMq");
            var factory = new ConnectionFactory();
            
            var rabbitMqUri = _configuration.GetValue<string>("RabbitMqUri");

            factory.Uri = new Uri(rabbitMqUri);
            //factory.Uri = "amqp://user:pass@hostName:port/vhost";

            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                Console.WriteLine(" started queue... ");
                channel.QueueDeclare(queue: "orders",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(order);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "orders",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine("Message sent : {0}", message);
            }
        }

        private async Task PublishOrderUsingAzureServiceBus(Order order)
        {
            var orderJson = JsonConvert.SerializeObject(order);

            var queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            var message = new Message
            {
                Label = $"New Order at {new DateTimeOffset().LocalDateTime}",
                Body = Encoding.UTF8.GetBytes(orderJson)
            };

            await queueClient.SendAsync(message);

            await queueClient.CloseAsync();
        }

        private Order CreateOrder(List<BasketItem> basketItems)
        {
           var order = new Order();

            foreach (var item in basketItems)
            {
                order.ProductsId.Add(item.Id);
                //order.TotalPrice += item. // TotalPrice could be fetched later by Order API.
            }

            return order;
        }

        // GET: api/BasketItems
        [HttpGet]
        public IEnumerable<BasketItem> GetBasketItem()
        {
            return _redisCacheClient.GetAllBasketItems();
            //return _context.BasketItem;
        }

        // POST: api/BasketItems
        [HttpPost]
        public async Task<IActionResult> PostBasketItem([FromBody] BasketItem basketItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _redisCacheClient.AddBasketItem(basketItem);
            //_context.BasketItem.Add(basketItem);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetBasketItem", new { id = basketItem.Id }, basketItem);
        }
    }
}