namespace idou.Core.Domain;

public class ChangeFilter(IReadOnlyCollection<EntityType> entityTypes, DateTimeOffset? since)
{
    public IReadOnlyCollection<EntityType> EntityTypes { get; } = entityTypes;
    public DateTimeOffset? Since { get; } = since;
}