namespace idou.Core.Domain;

public class EntityRecord
{
    public EntityKey Key { get; }
    public EntityType Type { get; }
    public IDictionary<string, object?> Attributes { get; }
}