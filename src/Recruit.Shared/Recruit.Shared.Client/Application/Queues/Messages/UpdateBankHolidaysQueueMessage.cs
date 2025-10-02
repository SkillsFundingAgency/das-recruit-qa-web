using System;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public class UpdateBankHolidaysQueueMessage
{
    public DateTime? CreatedByScheduleDate { get; set; }
}