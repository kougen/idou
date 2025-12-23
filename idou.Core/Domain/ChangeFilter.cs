namespace idou.Core.Domain;

public class ChangeFilter
{
    public IReadOnlyCollection<EntityType> EntityTypes { get; }
    public DateTimeOffset? Since { get; }
}