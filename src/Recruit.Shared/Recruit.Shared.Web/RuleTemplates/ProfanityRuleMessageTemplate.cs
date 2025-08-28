using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Shared.Web.RuleTemplates;

public static class ProfanityRuleMessageTemplate
{
    public static string ToText(ProfanityData msgData, string fieldName)
    {
        var baseMessage = $"{fieldName} contains the phrase '{msgData.Profanity}'";
        return msgData.Occurrences > 1 
            ? $"{baseMessage} {msgData.Occurrences} times"
            : $"{baseMessage}";
    }
}