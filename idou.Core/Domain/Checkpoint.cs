namespace idou.Core.Domain;

public class Checkpoint(string token, DateTimeOffset? observedAt = null)
{
    public string Token { get; } = token;
    public DateTimeOffset ObservedAt { get; } = observedAt ?? DateTimeOffset.UtcNow;
}