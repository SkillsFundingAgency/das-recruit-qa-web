using System;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public class TransferVacancyToLegalEntityQueueMessage
{
    public long VacancyReference { get; set; }
    public Guid UserRef { get; set; }
    public string UserEmailAddress { get; set; }
    public string UserName { get; set; }
    public TransferReason TransferReason { get; set; }
}