using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Shared.Web.Extensions;

public static class ApprenticeshipTypesExtensions
{
    public static bool IsFoundation(this ApprenticeshipTypes? apprenticeshipType)
    {
        return (apprenticeshipType ?? ApprenticeshipTypes.Standard).IsFoundation();
    }
    
    public static bool IsFoundation(this ApprenticeshipTypes apprenticeshipType)
    {
        return apprenticeshipType == ApprenticeshipTypes.Foundation;
    }
}