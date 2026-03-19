using System;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Responses;

public class BlockedOrganisationSummaryDto
{
    public string BlockedOrganisationId { get; set; }
    public DateTime BlockedDate { get; set; }
    public string BlockedByUser { get; set; }

    public static explicit operator BlockedOrganisationSummaryDto(BlockedOrganisationSummary source)
    {
        return new BlockedOrganisationSummaryDto
        {
            BlockedOrganisationId = source.BlockedOrganisationId,
            BlockedDate = source.BlockedDate,
            BlockedByUser = source.BlockedByUser
        };
    }

    public static explicit operator BlockedOrganisationSummary(BlockedOrganisationSummaryDto source)
    {
        return new BlockedOrganisationSummary
        {
            BlockedOrganisationId = source.BlockedOrganisationId,
            BlockedDate = source.BlockedDate,
            BlockedByUser = source.BlockedByUser
        };
    }
}