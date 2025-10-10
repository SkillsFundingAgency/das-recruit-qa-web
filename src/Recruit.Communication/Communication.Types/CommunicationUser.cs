using MongoDB.Bson.Serialization.Attributes;

namespace Recruit.Communication.Types;

/// <summary>
/// an end user that may receive a communication message
/// (depending on their preferences they may be filtered out of the final recipient list)
/// </summary>
public class CommunicationUser(
    string userId,
    string email,
    string name,
    string userType,
    UserParticipation participation,
    string dfEUserId)
{
    public string UserId { get; } = userId;
    public string Email { get; } = email;
    public string Name { get; } = name;

    /// This will be used to resolve UserPreferencesProvider
    /// This has to be a unique value across systems
    /// example values: VacancyServices.Recruit.Employer, VacancyServices.Faa.Candidates
    public string UserType { get; } = userType;

    public UserParticipation Participation { get; } = participation;

    [BsonDefaultValue("")]
    public string DfEUserId { get; } = dfEUserId;
}