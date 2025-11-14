using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Recruit.Communication.Types;
using Recruit.Communication.Types.Interfaces;

namespace Recruit.Vacancies.Client.Application.Communications.ParticipantResolverPlugins;

public class ProviderParticipantsResolverPlugin(
    IUserRepository userRepository,
    ILogger<ProviderParticipantsResolverPlugin> logger)
    : IParticipantResolver
{
    public string ParticipantResolverName => CommunicationConstants.ParticipantResolverNames.ProviderParticipantsResolverName;

    public async Task<IEnumerable<CommunicationUser>> GetParticipantsAsync(CommunicationRequest request)
    {
        logger.LogInformation($"Resolving participants for RequestType: '{request.RequestType}'");
        var entityId = request.Entities.Single(e => e.EntityType == CommunicationConstants.EntityTypes.Provider).EntityId.ToString();
        if(long.TryParse(entityId, out var ukprn) == false)
        {
            logger.LogInformation($"entity id: {entityId} is invalid ukprn for RequestType: '{request.RequestType}' and request id: {request.RequestId}");
            return Array.Empty<CommunicationUser>();
        }
        var users = await userRepository.GetProviderUsersAsync(ukprn);
        return ParticipantResolverPluginHelper.ConvertToCommunicationUsers(users, null);
    }

    public Task<IEnumerable<CommunicationMessage>> ValidateParticipantAsync(IEnumerable<CommunicationMessage> messages)
    {
        throw new NotImplementedException();
    }
}