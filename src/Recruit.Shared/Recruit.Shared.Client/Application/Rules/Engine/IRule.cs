using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Rules.Engine;

public interface IRule<in TSubject>
{
    Task<RuleOutcome> EvaluateAsync(TSubject subject);
}