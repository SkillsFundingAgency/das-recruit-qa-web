using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class AssignVacancyReviewCommand : ICommand, IRequest<Unit>
{
    public VacancyUser User { get; internal set; }
    public Guid? ReviewId { get; internal set; }
}