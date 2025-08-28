using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

public record GetPostcodeDataResponse(string Query, PostcodeData Result);