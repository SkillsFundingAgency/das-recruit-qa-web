using System;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public struct UpdateProvidersQueueMessage
{
    public DateTime? CreatedByScheduleDate { get; set; }
}