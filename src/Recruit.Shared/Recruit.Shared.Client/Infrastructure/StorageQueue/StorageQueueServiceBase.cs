using System.Threading.Tasks;
using Azure.Storage.Queues;
using Newtonsoft.Json;

namespace Recruit.Vacancies.Client.Infrastructure.StorageQueue;

public abstract class StorageQueueServiceBase
{
    protected abstract string ConnectionString { get; }

    protected async Task AddMessageToQueueAsync<T>(QueueClient queue, T message)
    {
        await queue.CreateIfNotExistsAsync();

        var json = JsonConvert.SerializeObject(message, Formatting.Indented);

        await queue.SendMessageAsync(json);
    }

    public abstract Task AddMessageAsync<T>(T message);
}