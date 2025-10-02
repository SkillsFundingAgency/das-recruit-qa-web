using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class UnblockProviderCommand : ICommand, IRequest<Unit>
{
    public long Ukprn { get; private set; }
    public VacancyUser QaVacancyUser { get; private set; }
    public DateTime UnblockedDate { get; private set; }
    public UnblockProviderCommand(long ukprn, VacancyUser qaVacancyUser, DateTime unblockedDate)
    {
        Ukprn = ukprn;
        QaVacancyUser = qaVacancyUser;
        UnblockedDate = unblockedDate;
    }
}