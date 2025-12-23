namespace idou.Core.Domain;

public class Checkpoint
{
    public string Token { get; }
    DateTimeOffset ObservedAt { get; }
}