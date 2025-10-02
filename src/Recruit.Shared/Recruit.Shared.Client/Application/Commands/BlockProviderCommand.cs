using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class BlockProviderCommand : ICommand, IRequest<Unit>
{
    public long Ukprn { get; private set; }
    public VacancyUser QaVacancyUser { get; private set; }
    public DateTime BlockedDate { get; private set; }
    public string Reason { get; private set; }
    public BlockProviderCommand(long ukprn, VacancyUser qaVacancyUser, DateTime blockedDate, string blockReason)
    {
        Ukprn = ukprn;
        QaVacancyUser = qaVacancyUser;
        BlockedDate = blockedDate;
        Reason = blockReason;
    }
}