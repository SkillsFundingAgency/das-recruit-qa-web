using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.VacancyApplications;

namespace Recruit.Shared.Web.Extensions;

public static class ApplicationReviewExtensions
{
    public static string GetFriendlyId(this ApplicationReview applicationReview)
    {
        return GetFriendlyId(applicationReview.Id);
    }

    public static string GetFriendlyId(this VacancyApplication vacancyApplication)
    {
        return GetFriendlyId(vacancyApplication.ApplicationReviewId);
    }

    private static string GetFriendlyId(Guid id)
    {
        return id.ToString().Replace("-", "").Substring(0, 7).ToUpperInvariant();
    }
}