using System;
using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewByFilterRequest(List<ReviewStatus>? status = null, DateTime? expiredAssignationDateTime = null) : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            return status != null 
                ? $"VacancyReviews?reviewStatus={string.Join("&reviewStatus=", status)}&expiredAssignationDateTime={expiredAssignationDateTime}" 
                : $"VacancyReviews?reviewStatus=&expiredAssignationDateTime={expiredAssignationDateTime}";
        }
    }
}