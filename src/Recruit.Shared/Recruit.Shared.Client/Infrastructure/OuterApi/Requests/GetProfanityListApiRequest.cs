using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record GetProfanityListApiRequest : IGetApiRequest
{
    public string GetUrl => "prohibitedContent/profanity";
}
