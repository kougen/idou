namespace idou.Core.Domain;

public class Checkpoint(string token)
{
    public string Token { get; } = token;
    public DateTimeOffset ObservedAt { get; } = DateTimeOffset.UtcNow;
}