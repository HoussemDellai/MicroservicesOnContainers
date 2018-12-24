
using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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

        //[Singleton]
        //public void BlobTrigger(
        //    [BlobTrigger("test")] string blob, ILogger logger)
        //{
        //    _sampleServiceB.DoIt();
        //    logger.LogInformation("Processed blob: " + blob);
        //}

        //public void BlobPoisonBlobHandler(
        //    [QueueTrigger("webjobs-blobtrigger-poison")] JObject blobInfo, ILogger logger)
        //{
        //    string container = (string)blobInfo["ContainerName"];
        //    string blobName = (string)blobInfo["BlobName"];

        //    logger.LogInformation($"Poison blob: {container}/{blobName}");
        //}

        //[WorkItemValidator]
        //public void ProcessWorkItem(
        //    [QueueTrigger("test")] WorkItem workItem, ILogger logger)
        //{
        //    _sampleServiceA.DoIt();
        //    logger.LogInformation($"Processed work item {workItem.ID}");
        //}

        public async Task ProcessWorkItem_ServiceBus([ServiceBusTrigger("ordersqueue", Connection = "ServiceBusConnectionString")]
            string item,
            //Order item,
            //string messageId,
            //int deliveryCount,
            ILogger log)
        {
            //log.LogInformation($"Processing ServiceBus message (Id={messageId}, DeliveryCount={deliveryCount})");

            await Task.Delay(1000);

            //log.LogInformation($"Message complete (Id={messageId})");
        }

        //public void ProcessEvents([EventHubTrigger("testhub2", Connection = "TestEventHubConnection")] EventData[] events,
        //    ILogger log)
        //{
        //    foreach (var evt in events)
        //    {
        //        log.LogInformation($"Event processed (Offset={evt.SystemProperties.Offset}, SequenceNumber={evt.SystemProperties.SequenceNumber})");
        //    }
        //}
    }
}
