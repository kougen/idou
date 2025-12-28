using idou.Core.Domain;
using idou.Core.Domain.Enums;

namespace idou.Core.Tests.Domain;

public class ChangeEventTests
{
    [Fact]
    public void ChangeEvent_Creation_Works()
    {
        var upsertMode = ChangeOperation.Upsert;
        var offset = DateTimeOffset.UtcNow;

        var changeEvent = new ChangeEvent(offset)
        {
            Operation = upsertMode,
            Key = new EntityKey("TestKey"),
            Type = new EntityType("TestType")
        };

        Assert.NotNull(changeEvent);
        Assert.Equal(upsertMode, changeEvent.Operation);
        Assert.Equal("TestKey", changeEvent.Key.Value);
        Assert.Equal("TestType", changeEvent.Type.ToString());
        Assert.Equal(offset, changeEvent.Timestamp);
    }
}