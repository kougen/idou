using idou.Core.Domain.Enums;

namespace idou.Core.Engine;

public interface IJobStateStore
{
    JobStatus GetStatus(string jobId);
    JobProgress GetProgress(string jobId);
    void SetStatus(string jobId, JobStatus status);
    void UpdateProgress(string jobId, JobProgress progress);
    void AppendLog(string jobId, string message);
    IReadOnlyList<string> ReadLogs(string jobId, int tail);
}