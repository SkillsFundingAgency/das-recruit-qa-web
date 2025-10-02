using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class GetVacancyPreviewApiRequest(int standardId) : IGetApiRequest
{
    public string GetUrl => $"vacancypreview?standardId={standardId}";
}