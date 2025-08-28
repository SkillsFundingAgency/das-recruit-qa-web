using System;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public class CommunicationsHouseKeepingQueueMessage
{
    public DateTime? CreatedByScheduleDate { get; set; }
}