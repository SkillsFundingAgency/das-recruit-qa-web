using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Application.CommandHandlers;

public class BlockProviderCommandHandler(
    ILogger<BlockProviderCommandHandler> logger,
    IBlockedOrganisationQuery blockedOrganisationQuery,
    IBlockedOrganisationRepository blockedOrganisationRepository,
    IMessaging messaging)
    : IRequestHandler<BlockProviderCommand, Unit>
{
    public async Task<Unit> Handle(BlockProviderCommand message, CancellationToken cancellationToken)
    {
        var blockedOrg = await blockedOrganisationQuery.GetByOrganisationIdAsync(message.Ukprn.ToString());
        if (blockedOrg?.BlockedStatus == BlockedStatus.Blocked)
        {
            logger.LogWarning($"Ignoring request to block provider with ukprn {message.Ukprn} as the provider is already blocked.");
            return Unit.Value;
        }

        await blockedOrganisationRepository.CreateAsync(ConvertToBlockedOrganisation(message));

        await messaging.PublishEvent(new ProviderBlockedEvent()
        {
            Ukprn = message.Ukprn,
            BlockedDate = message.BlockedDate,
            QaVacancyUser = message.QaVacancyUser
        });
        return Unit.Value;
    }

    private static BlockedOrganisation ConvertToBlockedOrganisation(BlockProviderCommand message)
    {
        return new BlockedOrganisation()
        {
            BlockedStatus = BlockedStatus.Blocked,
            OrganisationType = OrganisationType.Provider,
            OrganisationId = message.Ukprn.ToString(),
            UpdatedByUser = message.QaVacancyUser,
            UpdatedDate = message.BlockedDate,
            Reason = message.Reason
        };
    }
}