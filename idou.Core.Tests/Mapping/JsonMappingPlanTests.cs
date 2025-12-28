using idou.Core.Mapping;

namespace idou.Core.Tests.Mapping;

public class JsonMappingPlanTests
{
    [Fact]
    public void MappingPlanIsValid_WhenTheFileIsCorrectAndHasMappings()
    {
        var plan = new JsonMappingPlan("Mapping/assets/valid.json");

        Assert.True(plan.IsValid);
    }

    [Fact]
    public void Throws_WhenTheFileIsInvalid()
    {
        Assert.Throws<FileNotFoundException>(() => new JsonMappingPlan("Mapping/assets/invalid.json"));
    }

    [Fact]
    public void MappingPlanIsInvalid_WhenTheMappingsAreEmpty()
    {
        var plan = new JsonMappingPlan("Mapping/assets/empty.json");

        Assert.False(plan.IsValid);
    }
}