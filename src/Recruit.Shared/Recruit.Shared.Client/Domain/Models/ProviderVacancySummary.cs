using System;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Domain.Models;

public class ProviderVacancySummary
{
    public Guid Id { get; set; }
    public OwnerType? VacancyOwner { get; set; }
    public string EmployerAccountId { get; set; }
    public long VacancyReference { get; set; }
    public VacancyStatus Status { get; set; }
}