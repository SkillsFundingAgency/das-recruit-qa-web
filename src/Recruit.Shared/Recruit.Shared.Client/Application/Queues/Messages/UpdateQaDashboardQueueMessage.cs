using System;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public class UpdateQaDashboardQueueMessage
{
    public DateTime? CreatedByScheduleDate { get; set; }
}