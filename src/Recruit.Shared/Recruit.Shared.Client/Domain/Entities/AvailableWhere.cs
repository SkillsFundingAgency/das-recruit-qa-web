using System.Text.Json.Serialization;

namespace Recruit.Vacancies.Client.Domain.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AvailableWhere
{
    OneLocation,
    MultipleLocations,
    AcrossEngland,
}