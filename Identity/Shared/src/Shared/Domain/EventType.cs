namespace Shared.Domain;

public class EventType
{
    private const int _minElements = 5;
    private const int _maxElements = 6;

    private const int _organizationIndex = 0;
    private const int _serviceIndex      = 1;
    private const int _typeIndex         = 2;
    private const int _entityIndex       = 3;
    private const int _subentityIndex    = 4;

    public string  Name         { get; set; }
    public string  Organization { get; set; }
    public string  Service      { get; set; }
    public string  Type         { get; set; }
    public string  Entity       { get; set; }
    public string? SubEntity    { get; set; }
    public string? Status       { get; set; }
    public string  Version      { get; set; }

    public EventType()
    {
    }

    public EventType(string fullQualifiedName, int version = 1)
    {
        var topicParts = fullQualifiedName.Split(".");

        EnsureMaxParts(topicParts);
        EnsureMinPartsCount(topicParts);

        Name         = fullQualifiedName;
        Organization = topicParts[_organizationIndex];
        Service      = topicParts[_serviceIndex];
        Type         = topicParts[_typeIndex];
        Entity       = topicParts[_entityIndex];
        SubEntity    = topicParts.Length == _maxElements ? topicParts[_subentityIndex] : null;
        Status       = topicParts.Last();
        Version      = version.ToString();
    }

    private static void EnsureMinPartsCount(string[] topicParts)
    {
        if (topicParts.Length < _minElements)
        {
            throw new ArgumentException("Too many parts in messageType " + topicParts);
        }
    }

    private static void EnsureMaxParts(string[] topicParts)
    {
        if (topicParts.Length > _maxElements)
        {
            throw new ArgumentException("Insufficient parts in messageType " + topicParts);
        }
    }
}
