using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Communication.Types;
using Recruit.Communication.Types.Interfaces;

namespace Recruit.Communication.Core;

public abstract class CommunicationProcessorBase
{
    private readonly IDictionary<string, IUserPreferencesProvider> _userPreferencesProviders = new Dictionary<string, IUserPreferencesProvider>();

    public CommunicationProcessorBase(IEnumerable<IUserPreferencesProvider> userPreferencesProviders)
    {
        foreach (var plugin in userPreferencesProviders) _userPreferencesProviders.Add(plugin.UserType, plugin);
    }

    protected async Task<IEnumerable<Participant>> GetPreferencesForParticipantsAsync(string requestType, IEnumerable<CommunicationUser> users)
    {
        var participants = new List<Participant>();

        foreach (var user in users)
        {
            var provider = _userPreferencesProviders[user.UserType];

            var recipientPreferenceForRequestType = await provider.GetUserPreferenceAsync(requestType, user);

            var participant = new Participant() { User = user, Preferences = recipientPreferenceForRequestType };

            participants.Add(participant);
        }

        return participants;
    }
}