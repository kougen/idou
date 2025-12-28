using idou.Core.Domain;

namespace idou.Core.Tests.Domain;

public class EntityKeyTests
{
    [Fact]
    public void EntityKey_Creation_Works()
    {
        const string keyName = "TestKey";

        var entityKey = new EntityKey(keyName);

        Assert.Equal(keyName, entityKey.Value);
        Assert.Equal(keyName, entityKey.ToString());
        Assert.Equal(keyName.GetHashCode(), entityKey.GetHashCode());
        Assert.True(entityKey.Equals(new EntityKey(keyName)));
        Assert.False(entityKey.Equals(new EntityKey("DifferentKey")));
    }
}