using System.Text.Json.Serialization;

namespace Recruit.Vacancies.Client.Domain.Entities;

/// <summary>
/// overall decision for a rule set evaluation
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleSetDecision
{
    Unknown = 0,
    Refer,
    Approve,
    Indeterminate
}