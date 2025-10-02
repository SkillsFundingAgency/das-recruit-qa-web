using System;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Providers;

public interface IMinimumWageProvider
{
    MinimumWage GetWagePeriod(DateTime date);
}