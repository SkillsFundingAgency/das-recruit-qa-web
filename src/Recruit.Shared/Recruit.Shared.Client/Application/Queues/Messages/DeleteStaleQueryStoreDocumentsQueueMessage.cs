using System;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public class DeleteStaleQueryStoreDocumentsQueueMessage
{
    public DateTime? CreatedByScheduleDate { get; set; }
}