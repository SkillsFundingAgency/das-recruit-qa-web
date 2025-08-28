using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record GetEmployerDashboardVacanciesApiRequest(long AccountId,int PageNumber, List<ApplicationReviewStatus> ApplicationReviewStatuses) : IGetApiRequest
{
    public string GetUrl => $"employerAccounts/{AccountId}/dashboard/vacancies?pageNumber={PageNumber}&status={string.Join("&status=",ApplicationReviewStatuses)}";
}