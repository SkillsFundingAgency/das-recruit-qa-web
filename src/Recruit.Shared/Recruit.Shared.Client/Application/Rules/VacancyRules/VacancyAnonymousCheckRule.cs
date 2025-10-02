using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Rules.Engine;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Rules.VacancyRules;

public sealed class VacancyAnonymousCheckRule : Rule, IRule<Vacancy>
{
    public VacancyAnonymousCheckRule() : base(RuleId.VacancyAnonymous)
    {
            
    }

    public Task<RuleOutcome> EvaluateAsync(Vacancy subject)
    {
        var outcomeBuilder = RuleOutcomeDetailsBuilder.Create(RuleId);

        var score = subject.IsAnonymous ? 100 : 0;

        var outcomeResult = new RuleOutcome(
            RuleId, 
            score, 
            "Anonymous employer",
            nameof(Vacancy.EmployerName));

        var outcome = outcomeBuilder.Add(new [] { outcomeResult })
            .ComputeSum();

        return Task.FromResult(outcome);
    }
}