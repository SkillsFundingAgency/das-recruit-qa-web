using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Models;
using System.Linq;

namespace Recruit.Vacancies.Client.Infrastructure.Extensions;

public static class VacancyDtoMappingExtensions
{
    public static Vacancy ToVacancy(this VacancyDto dto) => new()
    {
        Id = dto.Id,
        VacancyReference = dto.VacancyReference,
        Status = dto.Status,
        ApprenticeshipType = dto.ApprenticeshipType,
        Title = dto.Title,
        OwnerType = dto.OwnerType ?? default,
        SourceOrigin = dto.SourceOrigin ?? default,
        SourceType = dto.SourceType ?? default,
        SourceVacancyReference = dto.SourceVacancyReference,
        ApprovedDate = dto.ApprovedDate,
        CreatedDate = dto.CreatedDate,
        LastUpdatedDate = dto.LastUpdatedDate,
        SubmittedDate = dto.SubmittedDate,
        ReviewDate = dto.ReviewRequestedDate,
        ClosedDate = dto.ClosedDate,
        DeletedDate = dto.DeletedDate,
        LiveDate = dto.LiveDate,
        StartDate = dto.StartDate,
        ClosingDate = dto.ClosingDate,
        ReviewCount = dto.ReviewCount,
        ApplicationUrl = dto.ApplicationUrl,
        ApplicationMethod = dto.ApplicationMethod,
        ApplicationInstructions = dto.ApplicationInstructions,
        ShortDescription = dto.ShortDescription,
        Description = dto.Description,
        AnonymousReason = dto.AnonymousReason,
        DisabilityConfident = dto.DisabilityConfident == true
            ? DisabilityConfident.Yes
            : DisabilityConfident.No,
        EmployerContact = dto.Contact,
        EmployerDescription = dto.EmployerDescription,
        EmployerLocations = dto.EmployerLocations,
        EmployerLocationOption = dto.EmployerLocationOption,
        EmployerLocationInformation = dto.EmployerLocationInformation,
        EmployerName = dto.EmployerName,
        EmployerNameOption = dto.EmployerNameOption,
        EmployerRejectedReason = dto.EmployerRejectedReason,
        LegalEntityName = dto.LegalEntityName,
        EmployerWebsiteUrl = dto.EmployerWebsiteUrl,
        GeoCodeMethod = dto.GeoCodeMethod,
        NumberOfPositions = dto.NumberOfPositions,
        OutcomeDescription = dto.OutcomeDescription,
        ProgrammeId = dto.ProgrammeId,
        Skills = dto.Skills,
        Qualifications = dto.Qualifications,
        ThingsToConsider = dto.ThingsToConsider,
        TrainingDescription = dto.TrainingDescription,
        AdditionalTrainingDescription = dto.AdditionalTrainingDescription,
        TrainingProvider = dto.TrainingProvider,
        Wage = dto.Wage,
        ClosureReason = dto.ClosureReason,
        TransferInfo = dto.TransferInfo,
        AdditionalQuestion1 = dto.AdditionalQuestion1,
        AdditionalQuestion2 = dto.AdditionalQuestion2,
        HasSubmittedAdditionalQuestions = dto.HasSubmittedAdditionalQuestions ?? false,
        HasChosenProviderContactDetails = dto.HasChosenProviderContactDetails,
        HasOptedToAddQualifications = dto.HasOptedToAddQualifications,
        EmployerReviewFieldIndicators = dto.EmployerReviewFieldIndicators?
            .Select(x => new EmployerReviewFieldIndicator { FieldIdentifier = x.FieldIdentifier, IsChangeRequested = x.IsChangeRequested })
            .ToList(),
        ProviderReviewFieldIndicators = dto.ProviderReviewFieldIndicators?
            .Select(x => new ProviderReviewFieldIndicator { FieldIdentifier = x.FieldIdentifier, IsChangeRequested = x.IsChangeRequested })
            .ToList(),
    };
}
