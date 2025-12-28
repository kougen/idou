using idou.Core.Domain;

namespace idou.Core.Tests.Domain;

public class ChangeFilterTests
{
    [Fact]
    public void ChangeFilter_Constructor_SetsProperties()
    {
        var entityTypes = new List<EntityType>
        {
            new("TypeA"),
            new("TypeB")
        };
        var since = DateTimeOffset.UtcNow.AddDays(-1);

        var changeFilter = new ChangeFilter(entityTypes, since);

        Assert.Equal(entityTypes, changeFilter.EntityTypes);
        Assert.Equal(since, changeFilter.Since);
    }
}