using System;

namespace Recruit.Vacancies.Client.Domain.Entities;

public class RuleOutcomeIndicator
{
    public Guid RuleOutcomeId { get; set; }

    public bool IsReferred { get; set; }
}