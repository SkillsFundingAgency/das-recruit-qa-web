using System;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class PublishVacancyCommand : ICommand, IRequest<Unit>
{
    public Guid VacancyId { get; set; }
}