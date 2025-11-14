using Recruit.Vacancies.Client.Domain.Exceptions;

namespace Recruit.Qa.Web.Exceptions;

public class UnassignedVacancyReviewException(string message) : RecruitException(message);