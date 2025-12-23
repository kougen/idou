namespace idou.Core.Domain;

public class EntityKey(string value)
{
    public string Value { get; } = value;

    public override string ToString() => Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override bool Equals(object? obj) => obj is EntityKey key && Value == key.Value;
}