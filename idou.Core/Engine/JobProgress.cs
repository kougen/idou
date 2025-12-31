using idou.Core.Domain;

namespace idou.Core.Engine;

public class JobProgress
{
    public long ProcessedChanges { get; }
    public long AppliedChanges { get; }
    public long FailedChanges { get; }
    public DateTimeOffset? LastSuccessfulApplyAt { get; }
    public Checkpoint? LastCheckpoint { get; }
    public string? LastErrorMessage { get; }
}