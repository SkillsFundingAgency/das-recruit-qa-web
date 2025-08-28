using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Geocode;

public class Geocode
{
    public GeoCodeMethod GeoCodeMethod { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public override string ToString()
    {
        return $"{{\"Latitude\":{Latitude}, \"Longitude\":{Longitude}}}";
    }
}