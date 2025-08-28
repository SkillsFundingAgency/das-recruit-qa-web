using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class SetupEmployerCommand : ICommand, IRequest<Unit>
{
    public string EmployerAccountId { get; set; }
}