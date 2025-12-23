using idou.Core.Domain;

namespace idou.Core;

public interface ICheckpointStore
{
    Task<Checkpoint?> LoadAsync(string jobId, CancellationToken cancellationToken);
    Task SaveAsync(string jobId, Checkpoint checkpoint, CancellationToken cancellationToken);
}