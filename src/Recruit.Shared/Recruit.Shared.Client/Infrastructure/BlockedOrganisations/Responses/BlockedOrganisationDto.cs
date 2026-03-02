using System;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Responses;

public class BlockedOrganisationDto
{
    public Guid Id { get; set; }
    public OrganisationType OrganisationType { get; set; }
    public string OrganisationId { get; set; }
    public BlockedStatus BlockedStatus { get; set; }
    public VacancyUser UpdatedByUser { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Reason { get; set; }

    public static explicit operator BlockedOrganisationDto(BlockedOrganisation source)
    {
        return new BlockedOrganisationDto
        {
            Id = source.Id,
            OrganisationType = source.OrganisationType,
            OrganisationId = source.OrganisationId,
            BlockedStatus = source.BlockedStatus,
            UpdatedByUser = source.UpdatedByUser,
            UpdatedDate = source.UpdatedDate,
            Reason = source.Reason
        };
    }

    public static explicit operator BlockedOrganisation(BlockedOrganisationDto source)
    {
        return new BlockedOrganisation
        {
            Id = source.Id,
            OrganisationType = source.OrganisationType,
            OrganisationId = source.OrganisationId,
            BlockedStatus = source.BlockedStatus,
            UpdatedByUser = source.UpdatedByUser,
            UpdatedDate = source.UpdatedDate,
            Reason = source.Reason
        };
    }
}