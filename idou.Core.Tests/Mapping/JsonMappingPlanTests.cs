using idou.Core.Domain;
using idou.Core.Mapping;

namespace idou.Core.Tests.Mapping;

public class JsonMappingPlanTests
{
    [Fact]
    public void MappingPlanIsValid_WhenTheFileExistsAndHasMappings()
    {
        var plan = new JsonMappingPlan("Mapping/assets/valid.json");
        Assert.True(plan.IsValid);
    }

    [Fact]
    public void Throws_WhenTheFileDoesNotExist()
    {
        Assert.Throws<FileNotFoundException>(() => new JsonMappingPlan("Mapping/assets/does-not-exist.json"));
    }

    [Fact]
    public void MappingPlanIsInvalid_WhenTheMappingsAreEmpty()
    {
        var plan = new JsonMappingPlan("Mapping/assets/empty.json");
        Assert.False(plan.IsValid);
    }

    [Fact]
    public void MapsTheEntityType_WhenTheMappingExists()
    {
        var plan = new JsonMappingPlan("Mapping/assets/valid.json");

        var sourceType = new EntityType("users");
        var mapped = plan.MapEntityType(sourceType);

        Assert.True(plan.IsValid);
        Assert.Equal("accounts", mapped.Name);
    }

    [Fact]
    public void DoesNotMapTheEntityType_WhenNoMappingExists()
    {
        var plan = new JsonMappingPlan("Mapping/assets/valid.json");

        var sourceType = new EntityType("unknown");
        var mapped = plan.MapEntityType(sourceType);

        Assert.Equal(sourceType.Name, mapped.Name);
    }
}