using System;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Services.NextVacancyReview;

public interface INextVacancyReviewService
{
    Task<VacancyReview> GetNextVacancyReviewAsync(string email);
    DateTime GetExpiredAssignationDateTime();
    bool VacancyReviewCanBeAssigned(ReviewStatus reviewStatus, DateTime? reviewedDate);
}