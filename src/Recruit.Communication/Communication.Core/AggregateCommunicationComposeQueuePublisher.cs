using System.Threading.Tasks;
using Newtonsoft.Json;
using Azure.Storage.Queues;
using Recruit.Communication.Types;

namespace Communication.Core;

public class AggregateCommunicationComposeQueuePublisher(string storageConnectionString)
    : IAggregateCommunicationComposeQueuePublisher
{
    private const string QueueName = "aggregate-communication-composer-requests-queue";

    public async Task AddMessageAsync(AggregateCommunicationComposeRequest message)
    {
        var client = new QueueClient(storageConnectionString, QueueName);
            
        await client.CreateIfNotExistsAsync();

        var cloudMessage = JsonConvert.SerializeObject(message, Formatting.Indented);

        await client.SendMessageAsync(System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cloudMessage)));
    }
}