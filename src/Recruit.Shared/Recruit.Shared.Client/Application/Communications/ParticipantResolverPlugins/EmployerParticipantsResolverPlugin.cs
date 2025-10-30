using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Recruit.Communication.Types;
using Recruit.Communication.Types.Interfaces;

namespace Recruit.Vacancies.Client.Application.Communications.ParticipantResolverPlugins;

public class EmployerParticipantsResolverPlugin(
    IUserRepository userRepository,
    ILogger<EmployerParticipantsResolverPlugin> logger)
    : IParticipantResolver
{
    public string ParticipantResolverName => CommunicationConstants.ParticipantResolverNames.EmployerParticipantsResolverName;

    public async Task<IEnumerable<CommunicationUser>> GetParticipantsAsync(CommunicationRequest request)
    {
        logger.LogInformation($"Resolving participants for RequestType: '{request.RequestType}'");
        var employerAccountId = request.Entities.Single(e => e.EntityType == CommunicationConstants.EntityTypes.Employer).EntityId.ToString();
        if(string.IsNullOrWhiteSpace(employerAccountId))
        {
            return Array.Empty<CommunicationUser>();
        }
        var users = await userRepository.GetEmployerUsersAsync(employerAccountId);
        return ParticipantResolverPluginHelper.ConvertToCommunicationUsers(users, null);
    }

    public Task<IEnumerable<CommunicationMessage>> ValidateParticipantAsync(IEnumerable<CommunicationMessage> messages)
    {
        throw new NotImplementedException();
    }
}