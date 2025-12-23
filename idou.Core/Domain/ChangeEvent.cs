using idou.Core.Domain.Enums;

namespace idou.Core.Domain;

public record ChangeEvent
{
    public ChangeOperation Operation { get; init; }
    public EntityType Type { get; init; }
    public EntityKey Key { get; init; }
    public EntityRecord? Payload { get; init; }
    public string? Version { get; init; }
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
    public IDictionary<string, object?>? Metadata { get; init; }
}