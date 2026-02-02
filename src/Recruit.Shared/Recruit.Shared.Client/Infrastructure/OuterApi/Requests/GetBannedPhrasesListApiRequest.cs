using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record GetBannedPhrasesListApiRequest : IGetApiRequest
{
    public string GetUrl => "prohibitedContent/bannedPhrases";
}