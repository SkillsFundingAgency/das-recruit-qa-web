﻿using System;
using System.Linq;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Microsoft.Extensions.Logging;
using SFA.DAS.VacancyServices.Wage;

namespace Recruit.Vacancies.Client.Infrastructure.Services;

public class NationalMinimumWageProvider(ILogger<NationalMinimumWageProvider> logger) : IMinimumWageProvider
{
    public MinimumWage GetWagePeriod(DateTime date)
    {
        try
        {
            var minimumWages = NationalMinimumWageService.GetRatesAsync().Result;

            var wagePeriods = minimumWages.OrderBy(w => w.ValidFrom);

            MinimumWage currentWagePeriod = null;
            foreach (var wagePeriod in wagePeriods)
            {
                if (date.Date < wagePeriod.ValidFrom)
                    break;

                if (currentWagePeriod != null && currentWagePeriod.ValidFrom == wagePeriod.ValidFrom)
                    throw new InvalidOperationException($"Duplicate wage period: {currentWagePeriod.ValidFrom}");

                currentWagePeriod = new MinimumWage
                {
                    ValidFrom = wagePeriod.ValidFrom,
                    ApprenticeshipMinimumWage = wagePeriod.ApprenticeMinimumWage,
                    NationalMinimumWageLowerBound = wagePeriod.Under18NationalMinimumWage,
                    NationalMinimumWageUpperBound = wagePeriod.Over25NationalMinimumWage
                };
            }

            if (currentWagePeriod == null)
                throw new InvalidOperationException("Wage period is missing");

            return currentWagePeriod;
        }
        catch(InvalidOperationException ex)
        {
            logger.LogError(ex, $"Unable to find Wage Period for date: {date}");
                
            throw;
        }
    }
}