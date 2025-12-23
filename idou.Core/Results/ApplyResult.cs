namespace idou.Core.Results;

public class ApplyResult
{
    public int AppliedCount { get; }
    public int FailedCount { get; }
    public IReadOnlyList<string> Errors { get; }
}