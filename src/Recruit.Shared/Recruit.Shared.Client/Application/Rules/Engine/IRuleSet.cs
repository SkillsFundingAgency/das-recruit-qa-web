using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Rules.Engine;

internal interface IRuleSet<in TSubject>
{
    Task<RuleSetOutcome> EvaluateAsync(TSubject subject, RuleSetOptions options = null);
}