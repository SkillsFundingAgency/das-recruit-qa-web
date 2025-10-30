using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class BlockProviderCommand(long ukprn, VacancyUser qaVacancyUser, DateTime blockedDate, string blockReason)
    : ICommand, IRequest<Unit>
{
    public long Ukprn { get; private set; } = ukprn;
    public VacancyUser QaVacancyUser { get; private set; } = qaVacancyUser;
    public DateTime BlockedDate { get; private set; } = blockedDate;
    public string Reason { get; private set; } = blockReason;
}