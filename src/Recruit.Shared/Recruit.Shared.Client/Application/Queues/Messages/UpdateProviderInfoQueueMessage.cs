using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Application.Queues.Messages;

public class UpdateProviderInfoQueueMessage
{
    public List<long> Ukprns { get; set; }
}