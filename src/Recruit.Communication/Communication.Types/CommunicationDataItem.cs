namespace Recruit.Communication.Types;

public class CommunicationDataItem(string key, string value)
{
    public string Key { get; } = key;
    public string Value { get; } = value;
}