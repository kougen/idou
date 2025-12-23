namespace idou.Core.Domain;

public class ChangeBatch(IReadOnlyList<ChangeEvent> events, Checkpoint nextCheckpoint)
{
    public IReadOnlyList<ChangeEvent> Events { get; init; } = events;
    public Checkpoint NextCheckpoint { get; init; } = nextCheckpoint;
}