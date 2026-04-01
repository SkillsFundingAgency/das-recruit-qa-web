using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Queues;
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

        var queueClient = new QueueClient(ConnectionString, queueName);

        await AddMessageToQueueAsync(queueClient, message);
    }
}