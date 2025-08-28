using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class CloseVacancyCommand : ICommand, IRequest<Unit>
{
    public Guid VacancyId { get; }
    public VacancyUser User { get; }
    public ClosureReason ClosureReason { get; }

    public CloseVacancyCommand(Guid vacancyId, VacancyUser user, ClosureReason closureReason)
    {
        VacancyId = vacancyId;
        User = user;
        ClosureReason = closureReason;
    }
}