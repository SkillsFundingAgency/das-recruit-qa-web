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

public class UnblockProviderCommandHandler : IRequestHandler<UnblockProviderCommand, Unit>
{
    private readonly ILogger<UnblockProviderCommandHandler> _logger;
    private readonly IBlockedOrganisationQuery _blockedOrganisationQuery;
    private readonly IBlockedOrganisationRepository _blockedOrganisationRepository;
    private readonly IMessaging _messaging;
    public UnblockProviderCommandHandler(
        ILogger<UnblockProviderCommandHandler> logger,
        IBlockedOrganisationQuery blockedOrganisationQuery,
        IBlockedOrganisationRepository blockedOrganisationRepository,
        IMessaging messaging)
    {
        _logger = logger;
        _blockedOrganisationQuery = blockedOrganisationQuery;
        _blockedOrganisationRepository = blockedOrganisationRepository;
        _messaging = messaging;
    }
        
    public async Task<Unit> Handle(UnblockProviderCommand message, CancellationToken cancellationToken)
    {
        var blockedOrg = await _blockedOrganisationQuery.GetByOrganisationIdAsync(message.Ukprn.ToString());
        if (blockedOrg?.BlockedStatus == BlockedStatus.Blocked)
        {
            _logger.LogInformation($"Request to unblock provider with ukprn {message.Ukprn}.");
            await _blockedOrganisationRepository.CreateAsync(ConvertToBlockedOrganisation(message));

            await _messaging.PublishEvent(new ProviderBlockedEvent()
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