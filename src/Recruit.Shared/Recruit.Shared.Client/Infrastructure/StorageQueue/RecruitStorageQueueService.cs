using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Recruit.Vacancies.Client.Application.Queues;
using Recruit.Vacancies.Client.Application.Queues.Messages;
using Recruit.Vacancies.Client.Infrastructure.EventStore;

namespace Recruit.Vacancies.Client.Infrastructure.StorageQueue;

internal class RecruitStorageQueueService(string connString) : StorageQueueServiceBase, IRecruitQueueService
{
    private readonly Dictionary<Type, string> _messageToStorageQueueMapper = new()
    {
        { typeof(EventItem), QueueNames.DomainEventsQueueName },
        { typeof(ReportQueueMessage), QueueNames.ReportQueueName },
        { typeof(UpdateEmployerUserAccountQueueMessage), QueueNames.UpdateEmployerUserAccountQueueName },
        { typeof(CommunicationsHouseKeepingQueueMessage), QueueNames.CommunicationsHouseKeepingQueueName },
    };

    protected override string ConnectionString { get; } = connString;

    public override async Task AddMessageAsync<T>(T message)
    {
        if (!_messageToStorageQueueMapper.TryGetValue(typeof(T), out var queueName) ||
            string.IsNullOrWhiteSpace(queueName))
        {
            throw new ArgumentException($"Cannot map type {typeof(T).Name} to a queue name");
        }
        var storageAccount = CloudStorageAccount.Parse(ConnectionString);
        var client = storageAccount.CreateCloudQueueClient();

        var queue = client.GetQueueReference(queueName);

        await AddMessageToQueueAsync(queue, message);
    }
}