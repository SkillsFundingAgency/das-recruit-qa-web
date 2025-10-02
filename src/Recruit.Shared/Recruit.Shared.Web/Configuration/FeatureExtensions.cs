using Recruit.Vacancies.Client.Application.FeatureToggle;
using Microsoft.Extensions.DependencyInjection;

namespace Recruit.Shared.Web.Configuration;

public static class FeatureExtensions
{
    public static void AddFeatureToggle(this IServiceCollection services)
    {
        services.AddSingleton<IFeature, Feature>();
    }
}