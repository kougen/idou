using idou.Core.Domain.Enums;

namespace idou.Core.Domain;

public class ChangeEvent
{
    public ChangeOperation Operation { get; }
    public EntityType Type { get; }
    public EntityKey Key { get; }
    public EntityRecord? Payload { get; }
    public string? Version { get; }
    public DateTimeOffset Timestamp { get; }
    public IDictionary<string, object?>? Metadata { get; }
}