using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Queues;
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

    private readonly QueueClientOptions _queueClientOptions = new()
    {
        MessageEncoding = QueueMessageEncoding.Base64
    };

    protected override string ConnectionString { get; } = connString;

    public override async Task AddMessageAsync<T>(T message)
    {
        if (!_messageToStorageQueueMapper.TryGetValue(typeof(T), out var queueName) ||
            string.IsNullOrWhiteSpace(queueName))
        {
            throw new ArgumentException($"Cannot map type {typeof(T).Name} to a queue name");
        }

        var queueClient = new QueueClient(ConnectionString, queueName, _queueClientOptions);

        await AddMessageToQueueAsync(queueClient, message);
    }
}