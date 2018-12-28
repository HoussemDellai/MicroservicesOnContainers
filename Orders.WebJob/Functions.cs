using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orders.WebJob.Models;

namespace Orders.WebJob
{
    public class Functions
    {
        private readonly ISampleServiceA _sampleServiceA;
        private readonly ISampleServiceB _sampleServiceB;

        public Functions(ISampleServiceA sampleServiceA, ISampleServiceB sampleServiceB)
        {
            _sampleServiceA = sampleServiceA;
            _sampleServiceB = sampleServiceB;
        }

        public async Task ProcessWorkItem_ServiceBus([ServiceBusTrigger("ordersqueue", Connection = "ServiceBusConnectionString")]
            string orderJson,
            string messageId,
            int deliveryCount,
            ILogger log)
        {
            log.LogInformation($"Processing ServiceBus message (Id={messageId}, DeliveryCount={deliveryCount}, orderJson={orderJson})");

            var order = JsonConvert.DeserializeObject<Order>(orderJson);

            await Task.Delay(1000);

            log.LogInformation($"Message complete (Id={messageId}, orderJson={orderJson}, order.ProductsId.Count={order.ProductsId.Count})");
        }
    }
}
