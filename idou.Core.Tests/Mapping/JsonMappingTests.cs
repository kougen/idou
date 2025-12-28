using idou.Core.Mapping;

namespace idou.Core.Tests.Mapping;

public class JsonMappingTests
{
    [Fact]
    public void JsonMapping_Creation_Works()
    {
        const string source = "sourceField";
        const string target = "targetField";

        var mapping = new JsonMapping
        {
            Source = source,
            Target = target
        };

        Assert.Equal(source, mapping.Source);
        Assert.Equal(target, mapping.Target);
    }
}