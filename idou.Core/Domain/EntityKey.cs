namespace idou.Core.Domain;

public class EntityKey(string value)
{
    public string Value { get; } = value;
}