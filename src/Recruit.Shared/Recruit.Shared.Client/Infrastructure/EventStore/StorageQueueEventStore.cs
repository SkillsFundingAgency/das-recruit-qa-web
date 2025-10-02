using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Events;
using Recruit.Vacancies.Client.Application.Queues;
using Recruit.Vacancies.Client.Domain.Messaging;
using Newtonsoft.Json;

namespace Recruit.Vacancies.Client.Infrastructure.EventStore;

internal sealed class StorageQueueEventStore : IEventStore
{
    private readonly IRecruitQueueService _queue;
    public StorageQueueEventStore(IRecruitQueueService queue)
    {
        _queue = queue;
    }

    public Task Add(IEvent @event)
    {
        var json = JsonConvert.SerializeObject(@event, Formatting.Indented);

        var item = new EventItem
        {
            EventType = @event.GetType().Name,
            Data = json
        };

        return _queue.AddMessageAsync(item);
    }
}