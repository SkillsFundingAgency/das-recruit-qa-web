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

public class UnblockProviderCommandHandler(
    ILogger<UnblockProviderCommandHandler> logger,
    IBlockedOrganisationQuery blockedOrganisationQuery,
    IBlockedOrganisationRepository blockedOrganisationRepository,
    IMessaging messaging)
    : IRequestHandler<UnblockProviderCommand, Unit>
{
    public async Task<Unit> Handle(UnblockProviderCommand message, CancellationToken cancellationToken)
    {
        var blockedOrg = await blockedOrganisationQuery.GetByOrganisationIdAsync(message.Ukprn.ToString());
        if (blockedOrg?.BlockedStatus == BlockedStatus.Blocked)
        {
            logger.LogInformation($"Request to unblock provider with ukprn {message.Ukprn}.");
            await blockedOrganisationRepository.CreateAsync(ConvertToBlockedOrganisation(message));

            await messaging.PublishEvent(new ProviderBlockedEvent()
            {
                Ukprn = message.Ukprn,
                BlockedDate = message.UnblockedDate,
                QaVacancyUser = message.QaVacancyUser
            });
        }
        return Unit.Value;
    }

    private static BlockedOrganisation ConvertToBlockedOrganisation(UnblockProviderCommand message)
    {
        return new BlockedOrganisation()
        {
            BlockedStatus = BlockedStatus.Unblocked,
            OrganisationType = OrganisationType.Provider,
            OrganisationId = message.Ukprn.ToString(),
            UpdatedByUser = message.QaVacancyUser,
            UpdatedDate = message.UnblockedDate
        };
    }
}