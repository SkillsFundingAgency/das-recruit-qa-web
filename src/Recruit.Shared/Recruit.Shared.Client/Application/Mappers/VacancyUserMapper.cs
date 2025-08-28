using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Mappers;

public static class VacancyUserMapper
{
    public static VacancyUser MapFromUser(User user)
    {
        return new VacancyUser
        {
            UserId = user.IdamsUserId,
            Email = user.Email,
            Name = user.Name
        };
    }
}