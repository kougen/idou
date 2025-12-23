namespace idou.Core.Domain;

public class EntityType(string name)
{
    public string Name { get; } = name;

    public override string ToString() => Name;

    public override int GetHashCode() => Name.GetHashCode();

    public override bool Equals(object? obj) => obj is EntityType type && Name == type.Name;
}