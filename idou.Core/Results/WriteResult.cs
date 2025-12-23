namespace idou.Core.Results;

public class WriteResult
{
    public int WrittenCount { get; }
    public int FailedCount { get; }
    IReadOnlyList<string> Errors { get; }
}