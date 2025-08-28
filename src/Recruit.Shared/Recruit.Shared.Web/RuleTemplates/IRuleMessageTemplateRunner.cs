using Recruit.Vacancies.Client.Application.Rules;

namespace Recruit.Shared.Web.RuleTemplates;

public interface IRuleMessageTemplateRunner
{
    string ToText(RuleId ruleId, string data, string fieldName);
}