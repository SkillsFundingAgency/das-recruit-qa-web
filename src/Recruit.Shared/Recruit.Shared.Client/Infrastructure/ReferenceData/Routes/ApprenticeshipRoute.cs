using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

namespace Recruit.Vacancies.Client.Infrastructure.ReferenceData.Routes;

public class ApprenticeshipRoute : IApprenticeshipRoute
{
    public string Route { get; set; }
    public int Id { get; set; }

    public static implicit operator ApprenticeshipRoute(GetRouteResponse.GetRouteResponseItem source)
    {
        return new ApprenticeshipRoute
        {
            Id = source.Id,
            Route = source.Route,
        };
    }
}