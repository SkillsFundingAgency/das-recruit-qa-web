﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Recruit.Vacancies.Client.Application.Rules.Engine;
using Recruit.Vacancies.Client.Application.Rules.Extensions;
using Recruit.Vacancies.Client.Domain.Entities;
using Newtonsoft.Json;

namespace Recruit.Vacancies.Client.Application.Rules.BaseRules;

public abstract class BaseProfanityChecksRule(
    RuleId ruleId,
    ConsolidationOption consolidationOption,
    decimal weighting = 1.0m)
    : Rule(ruleId, weighting)
{
    protected IEnumerable<string> ProfanityList { get; set; } = new List<string>();

    protected IEnumerable<RuleOutcome> ProfanityCheckAsync(Expression<Func<string>> property, string relatedFieldId = null)
    {
        var fieldId = relatedFieldId ?? property.GetQualifiedFieldId();

        var foundProfanities = FindOccurrences(property);

        if (foundProfanities.Values.Sum() > 0)
        {
            switch (consolidationOption)
            {
                case ConsolidationOption.NoConsolidation:
                    return CreateUnconsolidatedOutcomes(foundProfanities, fieldId);

                case ConsolidationOption.ConsolidateByField:
                    return CreateConsolidatedOutcomes(foundProfanities, fieldId);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return new[] {CreateOutcome(0, $"No profanities found in '{fieldId}'", null, fieldId)};
    }

    private Dictionary<string, int> FindOccurrences(Expression<Func<string>> property)
    {
        var foundProfanities = new Dictionary<string, int>();
        var value = property.Compile()();
        if (string.IsNullOrWhiteSpace(value)) return foundProfanities;
        var checkValue = value.FormatForParsing();
            

        foreach (var profanity in ProfanityList)
        {
            var occurrences = checkValue.CountOccurrences(profanity);

            if (occurrences > 0)
            {
                if (!foundProfanities.ContainsKey(profanity)) foundProfanities.Add(profanity, 0);

                foundProfanities[profanity] += occurrences;
            }
        }

        return foundProfanities;
    }

    private IEnumerable<RuleOutcome> CreateUnconsolidatedOutcomes(Dictionary<string, int> foundProfanities, string fieldId)
    {
        return foundProfanities
            .Select(foundProfanity =>
            {
                var count = foundProfanity.Value;
                var term = foundProfanity.Key;
                var foundMessage = count > 1 ? $"found {count} times" : "found";
                var narrative = $"Profanity '{term}' {foundMessage} in '{fieldId}'";
                var data = JsonConvert.SerializeObject(new ProfanityData { Profanity = term, Occurrences = count });

                return CreateOutcome(count, narrative, data, fieldId);
            });
    }

    private IEnumerable<RuleOutcome> CreateConsolidatedOutcomes(Dictionary<string, int> foundProfanities, string fieldId)
    {
        var count = foundProfanities.Values.Sum();
        var terms = string.Join(",", foundProfanities.Keys);
        var narrative = $"{count} profanities '{terms}' found in '{fieldId}'";
        var data = JsonConvert.SerializeObject(new ProfanityData { Profanity = terms, Occurrences = count });

        return new[]
        {
            CreateOutcome(count, narrative, data, fieldId)
        };
    }
}