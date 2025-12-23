namespace idou.Core.Domain;

public record EntityRecord
{
    public required EntityKey Key { get; init; }
    public required EntityType Type { get; init; }
    public required IDictionary<string, object?> Attributes { get; init; }
}