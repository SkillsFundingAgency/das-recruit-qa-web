using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class SetupProviderEvent(long ukprn) : EventBase, INotification, IProviderEvent
{
    public long Ukprn { get; private set; } = ukprn;
}