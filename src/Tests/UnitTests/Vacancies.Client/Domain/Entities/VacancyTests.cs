﻿using System.Linq;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Xunit;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Domain.Entities;

public class VacancyTests
{
    [Fact]
    public void CanSubmit_ShouldNotSubmitDeletedVacancy()
    {
        var vacancy = new Vacancy
        {
            Status = VacancyStatus.Draft,
            IsDeleted = true
        };

        vacancy.CanSubmit.Should().BeFalse();
    }

    [Fact]
    public void CanSubmit_ShouldNotSubmitVacancyInUnexpectedStatus()
    {
        foreach (var status in Enum.GetValues(typeof(VacancyStatus)).Cast<VacancyStatus>()
                     .Except(new[] {VacancyStatus.Draft, VacancyStatus.Referred, VacancyStatus.Rejected, VacancyStatus.Review}))
        {
            var vacancy = new Vacancy
            {
                Status = status,
                IsDeleted = false
            };

            vacancy.CanSubmit.Should().BeFalse();
        }
    }
}