using idou.Core.Domain;
using idou.Core.Domain.Enums;

namespace idou.Core.Tests.Domain;

public class ChangeEventTests
{
    [Fact]
    public void ChangeEvent_Creation_Works()
    {
        const ChangeOperation upsertMode = ChangeOperation.Upsert;
        var offset = DateTimeOffset.UtcNow;
        var payload = new EntityRecord
        {
            Type = new EntityType("TestType"),
            Key = new EntityKey("TestKey"),
            Attributes = new Dictionary<string, object?>()
        };

        var changeEvent = new ChangeEvent(offset)
        {
            Operation = upsertMode,
            Key = new EntityKey("TestKey"),
            Type = new EntityType("TestType"),
            Version = "1.0.0",
            Payload = payload,
            Metadata = new Dictionary<string, object?>()
        };

        Assert.NotNull(changeEvent);
        Assert.Equal(upsertMode, changeEvent.Operation);
        Assert.Equal("TestKey", changeEvent.Key.Value);
        Assert.Equal("TestType", changeEvent.Type.ToString());
        Assert.Equal(offset, changeEvent.Timestamp);
        Assert.Equal("1.0.0", changeEvent.Version);
        Assert.Equal(payload, changeEvent.Payload);
        Assert.NotNull(changeEvent.Metadata);
    }
}