using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public interface IRecruitVacancyClient
{
    Task<Domain.Entities.User> GetUsersDetailsAsync(string userId);
    Task <Domain.Entities.User> GetUsersDetailsByDfEUserId(string dfeUserId);
}