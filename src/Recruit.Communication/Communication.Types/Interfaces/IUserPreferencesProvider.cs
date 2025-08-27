using System.Threading.Tasks;

namespace Recruit.Communication.Types.Interfaces;

public interface IUserPreferencesProvider
{
    string UserType { get; }
    Task<CommunicationUserPreference> GetUserPreferenceAsync(string requestType, CommunicationUser user);
}