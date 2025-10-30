namespace Recruit.Communication.Types;

public struct Entity(string entityType, object entityId)
{
    public string EntityType { get; } = entityType;
    public object EntityId { get; } = entityId;
}