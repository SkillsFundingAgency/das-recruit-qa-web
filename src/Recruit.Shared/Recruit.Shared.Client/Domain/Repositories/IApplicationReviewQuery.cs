using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IApplicationReviewQuery
{
    Task<List<T>> GetForVacancyAsync<T>(long vacancyReference);
    Task<List<ApplicationReview>> GetForCandidateAsync(Guid candidateId);
    Task<IEnumerable<long>> GetAllVacancyReferencesAsync();
}