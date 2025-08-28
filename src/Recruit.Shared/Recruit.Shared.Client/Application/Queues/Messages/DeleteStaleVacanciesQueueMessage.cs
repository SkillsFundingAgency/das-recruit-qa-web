using System;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public class DeleteStaleVacanciesQueueMessage
{
    public DateTime? CreatedByScheduleDate { get; set; }
}