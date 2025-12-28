using idou.Core.Domain;

namespace idou.Core.Tests.Domain;

public class EntityRecordTests
{
    [Fact]
    public void EntityRecord_Creation_Works()
    {
        var entityRecord = new EntityRecord
        {
            Key = new EntityKey("TestKey"),
            Type = new EntityType("TestType"),
            Attributes = new Dictionary<string, object?>()
        };

        Assert.NotNull(entityRecord);

        Assert.Equal("TestKey", entityRecord.Key.Value);
        Assert.Equal("TestType", entityRecord.Type.ToString());
        Assert.NotNull(entityRecord.Attributes);
    }
}