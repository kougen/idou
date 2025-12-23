namespace idou.Core.Domain;

public class ChangeBatch
{
    public IReadOnlyList<ChangeEvent> Events { get; init;  }
    public Checkpoint NextCheckpoint { get; init; }
}