using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Domain.Extensions;
using Recruit.Qa.Web.ViewModels;
using Recruit.Qa.Web.Extensions;
using Recruit.Qa.Web.Security;
using QaDashboard = Recruit.Vacancies.Client.Domain.Models.QaDashboard;

namespace Recruit.Qa.Web.Orchestrators;

public class DashboardOrchestrator(
    IQaVacancyClient vacancyClient,
    ITimeProvider timeProvider,
    UserAuthorizationService userAuthorizationService,
    IRecruitQaOuterApiVacancyClient recruitQaOuterApiVacancyClient)
{
    public async Task<DashboardViewModel> GetDashboardViewModelAsync(string searchTerm, ClaimsPrincipal user)
    {
        var vacancyUser = user.GetVacancyUser();
        var reviews = await recruitQaOuterApiVacancyClient.GetDashboardAsync();
        var vm = MapToViewModel(reviews);

        vm.IsUserAdmin = await userAuthorizationService.IsTeamLeadAsync(user);
        if (vm.IsUserAdmin)
        {
            var inProgressSummaries = await vacancyClient.GetVacancyReviewsInProgressAsync();
            var inProgressVacanciesVmTasks = inProgressSummaries.Select(v => MapToViewModelAsync(v, vacancyUser)).ToList();
            var inProgressVacanciesVm = await Task.WhenAll(inProgressVacanciesVmTasks);
            vm.InProgressVacancies = inProgressVacanciesVm.ToList();
        }

        if (string.IsNullOrEmpty(searchTerm)) return vm;

        vm.LastSearchTerm = searchTerm;
        var matchedVacancyReview = await vacancyClient.GetSearchResultAsync(searchTerm);

        if (matchedVacancyReview != null)
        {
            var matchVacancyReviewSearchVm = await MapToViewModelAsync(matchedVacancyReview, vacancyUser);
            vm.SearchResult = matchVacancyReviewSearchVm;
        }

        return vm;
    }

    private async Task<VacancyReviewSearchResultViewModel> MapToViewModelAsync(VacancyReview vacancyReview, VacancyUser vacancyUser)
    {
        var isAvailableForReview =
            vacancyClient.VacancyReviewCanBeAssigned(vacancyReview.Status, vacancyReview.ReviewedDate);

        var vacancy = await vacancyClient.GetVacancyAsync(vacancyReview.VacancyReference);

        return new VacancyReviewSearchResultViewModel
        {
            IsAssignedToLoggedInUser = vacancyUser.Email == vacancyReview.ReviewedByUser?.Email,
            AssignedTo = vacancyReview.ReviewedByUser?.Email,
            AssignedTimeElapsed = vacancyReview.ReviewedDate.GetShortTimeElapsed(timeProvider.Now),
            ClosingDate = vacancyReview.VacancySnapshot.ClosingDate.GetValueOrDefault(),
            EmployerName = vacancyReview.VacancySnapshot.LegalEntityName,
            VacancyReference = vacancyReview.VacancyReference.ToString(),
            VacancyTitle = vacancyReview.Title,
            ReviewId = vacancyReview.Id,
            IsClosed =  vacancyReview.Status == ReviewStatus.Closed,
            SubmittedDate = vacancyReview.VacancySnapshot.SubmittedDate.GetValueOrDefault(),
            IsAvailableForReview = isAvailableForReview,
            IsVacancyDeleted = vacancy.IsDeleted
        };
    }

    public async Task<Guid?> AssignNextVacancyReviewAsync(VacancyUser user)
    {
        await vacancyClient.AssignNextVacancyReviewAsync(user);

        var userVacancyReviews = await vacancyClient.GetAssignedVacancyReviewsForUserAsync(user.Email);

        return userVacancyReviews.FirstOrDefault()?.Id;
    }

    private static DashboardViewModel MapToViewModel(QaDashboard dashboard)
    {
        var vm = new DashboardViewModel
        {
            TotalVacanciesForReview = dashboard.TotalVacanciesForReview,
            TotalVacanciesBrokenSla = dashboard.TotalVacanciesBrokenSla,
            TotalVacanciesResubmitted = dashboard.TotalVacanciesResubmitted,
            TotalVacanciesSubmittedTwelveToTwentyFourHours = dashboard.TotalVacanciesSubmittedTwelveTwentyFourHours
        };

        return vm;
    }
}