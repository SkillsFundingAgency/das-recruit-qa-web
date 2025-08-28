using System;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public class DeleteReportsQueueMessage
{
    public DateTime? CreatedByScheduleDate { get; set; }
}