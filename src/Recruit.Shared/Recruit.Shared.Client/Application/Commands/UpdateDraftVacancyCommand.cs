using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class UpdateDraftVacancyCommand : ICommand, IRequest<Unit>
{
    public Vacancy Vacancy { get; set; }
    public VacancyUser User { get; set; }
}