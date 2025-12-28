using idou.Core.Domain;

namespace idou.Core.Tests.Domain;

public class EntityTypeTests
{
    [Fact]
    public void EntityType_Creation_Works()
    {
        const string typeName = "TestType";

        var entityType = new EntityType(typeName);

        Assert.Equal(typeName, entityType.ToString());
        Assert.Equal(typeName.GetHashCode(), entityType.GetHashCode());
        Assert.True(entityType.Equals(new EntityType(typeName)));
        Assert.False(entityType.Equals(new EntityType("DifferentType")));
    }
}