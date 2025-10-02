using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record GetProviderDashboardVacanciesApiRequest(long Ukprn,int PageNumber, List<ApplicationReviewStatus> ApplicationReviewStatuses) : IGetApiRequest
{
    public string GetUrl => $"providers/dashboard/{Ukprn}/vacancies?pageNumber={PageNumber}&status={string.Join("&status=",ApplicationReviewStatuses)}";
}