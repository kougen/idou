using idou.Core.Domain.Enums;

namespace idou.Core.Domain;

public record ChangeEvent
{
    public required ChangeOperation Operation { get; init; }
    public required EntityType Type { get; init; }
    public required EntityKey Key { get; init; }
    
    public DateTimeOffset Timestamp { get; }
    
    public EntityRecord? Payload { get; init; }
    public string? Version { get; init; }
    public IDictionary<string, object?>? Metadata { get; init; }

    public ChangeEvent(DateTimeOffset? timestamp = null)
    {
        Timestamp = timestamp ?? DateTimeOffset.UtcNow;
    }
}