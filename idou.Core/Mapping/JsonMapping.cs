namespace idou.Core.Mapping;

public record JsonMapping
{
    public required string Source { get; init; } = null!;
    public required string Target { get; init; } = null!;
}