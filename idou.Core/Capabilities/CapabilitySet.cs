namespace idou.Core.Capabilities;

public class CapabilitySet
{
    public bool SupportsChangeFeed { get; init; }
    public bool SupportsSnapshotRead { get; init; }
    public bool SupportsBulkWrite { get; init; }
    public bool SupportsTransactions { get; init; }
    public bool SupportsConditionalWrite { get; init; }

}