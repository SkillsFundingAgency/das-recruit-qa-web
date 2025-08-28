using Recruit.Vacancies.Client.Domain.Messaging;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Application.Events;

public interface IEventStore
{
    Task Add(IEvent @event);
}