using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Geocode;

public interface IOuterApiGeocodeService
{
    Task<Geocode> Geocode(string postcode);
}