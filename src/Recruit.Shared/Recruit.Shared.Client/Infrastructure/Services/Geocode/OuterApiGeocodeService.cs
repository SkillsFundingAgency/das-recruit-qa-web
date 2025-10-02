using Microsoft.Extensions.Logging;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.Services.Geocode.Request;
using Recruit.Vacancies.Client.Infrastructure.Services.Geocode.Responses;
using System;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Geocode;

public class OuterApiGeocodeService : IOuterApiGeocodeService
{
    private IRecruitOuterApiClient _outerApiClient;
    private ILogger<OuterApiGeocodeService> _logger;

    public OuterApiGeocodeService(IRecruitOuterApiClient outerApiClient, ILogger<OuterApiGeocodeService> logger)
    {
        _outerApiClient = outerApiClient;
        _logger = logger;
    }

    public async Task<Geocode> Geocode(string postcode)
    {
        try
        {
            _logger.LogInformation($"Getting geo code for postcode {postcode}");
            var result = await _outerApiClient.Get<GetGeoPointResponse>(new GetGeoCodeRequest(postcode));

            if (result?.GeoPoint?.Latitude != null && result?.GeoPoint?.Longitude != null)
            {
                return new Geocode
                {
                    GeoCodeMethod = Domain.Entities.GeoCodeMethod.OuterApi,
                    Latitude = result.GeoPoint.Latitude,
                    Longitude = result.GeoPoint.Longitude
                };
            }

            return null;
        }
        catch (Exception e)
        {
            string message = $"Get geocode failed for postcode: {postcode}";
            _logger.LogDebug(message);
            throw new Exception(message, e);
        }
    }
}