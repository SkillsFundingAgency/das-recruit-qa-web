using System;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class UnassignVacancyReviewCommand : ICommand, IRequest<Unit>
{
    public Guid ReviewId { get; set; }
}