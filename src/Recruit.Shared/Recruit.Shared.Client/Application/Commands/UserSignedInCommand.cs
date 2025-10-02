using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class UserSignedInCommand : ICommand, IRequest<Unit>
{
    public VacancyUser User { get; private set; }
    public UserType UserType { get; private set; }

    public UserSignedInCommand(VacancyUser vacancyUser, UserType userType)
    {
        User = vacancyUser;
        UserType = userType;
    }
}