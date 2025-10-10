using Recruit.Vacancies.Client.Domain.Repositories;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public class VacancyClient(
    IUserRepository userRepository)
    : IRecruitVacancyClient
{
    public Task<Domain.Entities.User> GetUsersDetailsAsync(string userId)
    {
        return userRepository.GetAsync(userId);
    }
        
    public Task<Domain.Entities.User> GetUsersDetailsByDfEUserId(string dfeUserId)
    {
        return userRepository.GetByDfEUserId(dfeUserId);
    }
}