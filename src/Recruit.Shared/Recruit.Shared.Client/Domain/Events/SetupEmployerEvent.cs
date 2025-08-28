using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class SetupEmployerEvent : EventBase, INotification, IEmployerEvent
{
    public string EmployerAccountId { get; set; }
}