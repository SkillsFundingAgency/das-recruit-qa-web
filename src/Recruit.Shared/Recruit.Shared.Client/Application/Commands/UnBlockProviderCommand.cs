using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class UnblockProviderCommand(long ukprn, VacancyUser qaVacancyUser, DateTime unblockedDate)
    : ICommand, IRequest<Unit>
{
    public long Ukprn { get; private set; } = ukprn;
    public VacancyUser QaVacancyUser { get; private set; } = qaVacancyUser;
    public DateTime UnblockedDate { get; private set; } = unblockedDate;
}