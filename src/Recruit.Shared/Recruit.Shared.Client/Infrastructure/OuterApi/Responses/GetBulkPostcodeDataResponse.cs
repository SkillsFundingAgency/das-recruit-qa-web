using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

public class GetBulkPostcodeDataResponse: List<GetBulkPostcodeDataItemResponse>;

public record GetBulkPostcodeDataItemResponse(string Query, PostcodeData Result);