using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Application.Services.NextVacancyReview;
using Recruit.Vacancies.Client.Application.Services.Reports;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public class QaVacancyClient(
    IVacancyReviewQuery vacancyReviewQuery,
    IVacancyRepository vacancyRepository,
    IApprenticeshipProgrammeProvider apprenticeshipProgrammesProvider,
    IMessaging messaging,
    INextVacancyReviewService nextVacancyReviewService,
    IReportRepository reportRepository,
    IReportService reportService)
    : IQaVacancyClient
{
    private readonly ManualQaOutcome[] _displayableReviewOutcomes = [ManualQaOutcome.Approved, ManualQaOutcome.Referred];

    public Task ReferVacancyReviewAsync(Guid reviewId, string manualQaComment, List<ManualQaFieldIndicator> manualQaFieldIndicators, List<Guid> selectedAutomatedQaRuleOutcomeIds)
    {
        return messaging.SendCommandAsync(new ReferVacancyReviewCommand(reviewId, manualQaComment, manualQaFieldIndicators, selectedAutomatedQaRuleOutcomeIds));
    }

    public Task<IApprenticeshipProgramme> GetApprenticeshipProgrammeAsync(string programmeId)
    {
        return apprenticeshipProgrammesProvider.GetApprenticeshipProgrammeAsync(programmeId);
    }

    public async Task<Domain.Entities.VacancyReview> GetSearchResultAsync(string searchTerm)
    {
        if (TryGetVacancyReference(searchTerm, out var vacancyReference) == false) return null;

        var review = await vacancyReviewQuery.GetLatestReviewByReferenceAsync(vacancyReference);

        if (review != null)
        {
            var isIneligibleReview = review.Status == ReviewStatus.New || review.ManualOutcome == ManualQaOutcome.Transferred;

            if (isIneligibleReview == false)
            {
                return review;
            }
        }

        return null;
    }

    private bool TryGetVacancyReference(string value, out long vacancyReference)
    {
        vacancyReference = 0;
        if (string.IsNullOrEmpty(value)) return false;

        var regex = new Regex(@"^(VAC)?(\d{10})$", RegexOptions.IgnoreCase);
        var result = regex.Match(value);
        if (result.Success)
        {
            vacancyReference = long.Parse(result.Groups[2].Value);
        }
        return result.Success;
    }

    public Task<List<Domain.Entities.VacancyReview>> GetVacancyReviewsInProgressAsync()
    {
        return vacancyReviewQuery.GetVacancyReviewsInProgressAsync(nextVacancyReviewService.GetExpiredAssignationDateTime());
    }

    public Task<Vacancy> GetVacancyAsync(long vacancyReference)
    {
        return vacancyRepository.GetVacancyAsync(vacancyReference);
    }

    public Task<Domain.Entities.VacancyReview> GetVacancyReviewAsync(Guid reviewId)
    {
        return vacancyReviewQuery.GetAsync(reviewId);
    }

    public Task AssignNextVacancyReviewAsync(VacancyUser user)
    {
        return messaging.SendCommandAsync(new AssignVacancyReviewCommand
        {
            User = user
        });
    }

    public Task AssignVacancyReviewAsync(VacancyUser user, Guid reviewId)
    {
        return messaging.SendCommandAsync(new AssignVacancyReviewCommand
        {
            User = user,
            ReviewId = reviewId
        });
    }

    public Task<int> GetApprovedCountAsync(string submittedByUserEmail)
    {
        return vacancyReviewQuery.GetApprovedCountAsync(submittedByUserEmail);
    }

    public Task<int> GetApprovedFirstTimeCountAsync(string submittedByUserEmail)
    {
        return vacancyReviewQuery.GetApprovedFirstTimeCountAsync(submittedByUserEmail);
    }

    public Task<List<Domain.Entities.VacancyReview>> GetAssignedVacancyReviewsForUserAsync(string userId)
    {
        return vacancyReviewQuery.GetAssignedForUserAsync(userId, nextVacancyReviewService.GetExpiredAssignationDateTime());
    }

    public bool VacancyReviewCanBeAssigned(Domain.Entities.VacancyReview review)
    {
        return VacancyReviewCanBeAssigned(review.Status, review.ReviewedDate);
    }

    public bool VacancyReviewCanBeAssigned(ReviewStatus status, DateTime? reviewedDate)
    {
        return nextVacancyReviewService.VacancyReviewCanBeAssigned(status, reviewedDate);
    }

    public Task UnassignVacancyReviewAsync(Guid reviewId)
    {
        return messaging.SendCommandAsync(new UnassignVacancyReviewCommand { ReviewId = reviewId });
    }

    public Task<Domain.Entities.VacancyReview> GetCurrentReferredVacancyReviewAsync(long vacancyReference)
    {
        return vacancyReviewQuery.GetCurrentReferredVacancyReviewAsync(vacancyReference);
    }

    public async Task<List<Domain.Entities.VacancyReview>> GetVacancyReviewHistoryAsync(long vacancyReference)
    {
        var allVacancyReviews = await vacancyReviewQuery.GetForVacancyAsync(vacancyReference);

        return allVacancyReviews.Where(r => r.Status == ReviewStatus.Closed && _displayableReviewOutcomes.Contains(r.ManualOutcome.Value))
            .OrderByDescending(r => r.ReviewedDate)
            .ToList();
    }

    public Task<int> GetAnonymousApprovedCountAsync(string accountLegalEntityPublicHashedId)
    {
        return vacancyReviewQuery.GetAnonymousApprovedCountAsync(accountLegalEntityPublicHashedId);
    }

    public async Task<Guid> CreateApplicationsReportAsync(DateTime fromDate, DateTime toDate, VacancyUser user, string reportName)
    {
        var reportId = Guid.NewGuid();

        var owner = new ReportOwner
        {
            OwnerType = ReportOwnerType.Qa
        };

        await messaging.SendCommandAsync(new CreateReportCommand(
            reportId,
            owner,
            ReportType.QaApplications,
            new Dictionary<string, object> {
                { ReportParameterName.FromDate, fromDate},
                { ReportParameterName.ToDate, toDate}
            },
            user,
            reportName)
        );

        return reportId;
    }

    public Task<List<ReportSummary>> GetReportsAsync()
    {
        return reportRepository.GetReportsForQaAsync<ReportSummary>();
    }

    public Task<Report> GetReportAsync(Guid reportId)
    {
        return reportRepository.GetReportAsync(reportId);
    }

    public async Task WriteReportAsCsv(Stream stream, Report report)
    {
        await reportService.WriteReportAsCsv(stream, report);
    }

    public Task IncrementReportDownloadCountAsync(Guid reportId)
    {
        return reportRepository.IncrementReportDownloadCountAsync(reportId);
    }

    public Task UpdateDraftVacancyAsync(Vacancy vacancy, VacancyUser user)
    {
        var command = new UpdateDraftVacancyCommand
        {
            Vacancy = vacancy,
            User = user
        };

        return messaging.SendCommandAsync(command);
    }
}