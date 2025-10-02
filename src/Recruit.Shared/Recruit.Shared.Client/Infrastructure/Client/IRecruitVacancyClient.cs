using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public interface IRecruitVacancyClient
{
    Task<Domain.Entities.VacancyReview> GetCurrentReferredVacancyReviewAsync(long vacancyReference);
    Task<Domain.Entities.User> GetUsersDetailsAsync(string userId);
    Task<GetUserAccountsResponse> GetEmployerIdentifiersAsync(string userId, string email);
    Task <Domain.Entities.User> GetUsersDetailsByDfEUserId(string dfeUserId);
}