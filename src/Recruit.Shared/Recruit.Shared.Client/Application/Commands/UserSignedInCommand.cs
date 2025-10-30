using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class UserSignedInCommand(VacancyUser vacancyUser, UserType userType) : ICommand, IRequest<Unit>
{
    public VacancyUser User { get; private set; } = vacancyUser;
    public UserType UserType { get; private set; } = userType;
}