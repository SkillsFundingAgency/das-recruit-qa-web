using System;

namespace Recruit.Vacancies.Client.Domain.Events.Interfaces;

public interface IVacancyEvent
{
    Guid VacancyId { get; }
}