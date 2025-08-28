using Recruit.Vacancies.Client.Infrastructure.OuterApi;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetAnonymousApprovedCountByAccountLegalEntity(long accountLegalEntityId) : IGetApiRequest
{
    public string GetUrl => $"accounts/{accountLegalEntityId}/vacancyreviews";
}