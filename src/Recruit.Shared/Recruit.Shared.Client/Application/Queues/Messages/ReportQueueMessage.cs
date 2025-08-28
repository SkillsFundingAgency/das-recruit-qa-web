using System;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public class ReportQueueMessage
{
    public Guid ReportId { get; set; }
}