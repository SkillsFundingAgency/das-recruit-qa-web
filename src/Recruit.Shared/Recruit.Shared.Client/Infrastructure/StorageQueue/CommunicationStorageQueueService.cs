using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Recruit.Communication.Types;

namespace Recruit.Vacancies.Client.Infrastructure.StorageQueue;

internal class CommunicationStorageQueueService(string connString) : StorageQueueServiceBase, ICommunicationQueueService
{
    private readonly IDictionary<Type, string> _messageToCommunicationStorageQueueMapper = new Dictionary<Type, string>
    {
        { typeof(CommunicationRequest), "communication-requests-queue" },
    };

    protected override string ConnectionString { get; } = connString;

    public override async Task AddMessageAsync<T>(T message)
    {
        var queueName = _messageToCommunicationStorageQueueMapper[typeof(T)];

        if(string.IsNullOrEmpty(queueName))
            throw new ArgumentException($"Cannot map type {typeof(T).Name} to a queue name");

        var storageAccount = CloudStorageAccount.Parse(ConnectionString);
        var client = storageAccount.CreateCloudQueueClient();

        var queue = client.GetQueueReference(queueName);

        await AddMessageToQueueAsync(queue, message);
    }
}