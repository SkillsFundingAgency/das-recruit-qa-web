using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.VacancyApplications;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Projections;

public class VacancyApplicationsProjectionService(
    ILogger<VacancyApplicationsProjectionService> logger,
    IVacancyRepository vacancyRepository,
    IApplicationReviewQuery applicationReviewQuery,
    IQueryStoreWriter writer)
    : IVacancyApplicationsProjectionService
{
    public async Task UpdateVacancyApplicationsAsync(long vacancyReference)
    {
        logger.LogInformation("Updating vacancyApplications projection for vacancyReference: {vacancyReference}", vacancyReference);

        var vacancy = await vacancyRepository.GetVacancyAsync(vacancyReference);
        var vacancyApplicationReviews = await applicationReviewQuery.GetForVacancyAsync<Domain.Entities.ApplicationReview>(vacancy.VacancyReference.Value);

        var vacancyApplications = new VacancyApplications
        {
            VacancyReference = vacancy.VacancyReference.Value,
            Applications = vacancyApplicationReviews.Select(MapToVacancyApplication).ToList()
        };

        await writer.UpdateVacancyApplicationsAsync(vacancyApplications);
    }

    private VacancyApplication MapToVacancyApplication(Domain.Entities.ApplicationReview review)
    {
        var projection = new VacancyApplication
        {
            CandidateId = review.CandidateId,
            Status = review.Status,
            SubmittedDate = review.SubmittedDate,
            ApplicationReviewId = review.Id,
            IsWithdrawn = review.IsWithdrawn,
            DisabilityStatus = ApplicationReviewDisabilityStatus.Unknown
        };

        if (review.IsWithdrawn == false)
        {
            projection.FirstName = review.Application.FirstName;
            projection.LastName = review.Application.LastName;
            projection.DateOfBirth = review.Application.BirthDate;
            projection.DisabilityStatus = review.Application.DisabilityStatus ?? ApplicationReviewDisabilityStatus.Unknown;
        }

        return projection;
    }
}