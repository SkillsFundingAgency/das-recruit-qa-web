using System;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Responses;

public class BlockedOrganisationDto
{
    public Guid Id { get; set; }
    public string OrganisationType { get; set; }
    public string OrganisationId { get; set; }
    public string BlockedStatus { get; set; }
    public string UpdatedByUserEmail { get; set; }
    public string UpdatedByUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Reason { get; set; }

    public static explicit operator BlockedOrganisationDto(BlockedOrganisation source)
    {
        return new BlockedOrganisationDto
        {
            Id = source.Id,
            OrganisationType = source.OrganisationType.ToString(),
            OrganisationId = source.OrganisationId,
            BlockedStatus = source.BlockedStatus.ToString(),
            UpdatedByUserEmail = source.UpdatedByUser.Email,
            UpdatedByUserId = source.UpdatedByUser.UserId ?? source.UpdatedByUser.DfEUserId,
            UpdatedDate = source.UpdatedDate,
            Reason = source.Reason
        };
    }

    public static explicit operator BlockedOrganisation(BlockedOrganisationDto source)
    {
        return new BlockedOrganisation
        {
            Id = source.Id,
            OrganisationType = Enum.Parse<OrganisationType>(source.OrganisationType),
            OrganisationId = source.OrganisationId,
            BlockedStatus = Enum.Parse<BlockedStatus>(source.BlockedStatus),
            UpdatedByUser = new VacancyUser
            {
                Email = source.UpdatedByUserEmail,
                UserId = source.UpdatedByUserId
            },
            UpdatedDate = source.UpdatedDate,
            Reason = source.Reason
        };
    }
}