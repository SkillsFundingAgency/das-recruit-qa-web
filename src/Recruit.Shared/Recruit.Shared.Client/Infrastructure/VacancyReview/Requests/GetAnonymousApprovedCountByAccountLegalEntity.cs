using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetAnonymousApprovedCountByAccountLegalEntity(long accountLegalEntityId) : IGetApiRequest
{
    public string GetUrl => $"accounts/{accountLegalEntityId}/vacancyreviews/count?status={ReviewStatus.Closed}&manualOutcome={ManualQaOutcome.Approved}&employerNameOption={EmployerNameOption.Anonymous}";
}