using idou.Core.Mapping;

namespace idou.Core.Tests.Mapping;

public class DefaultMappingPlanTests
{
    [Fact]
    public void DefaultMappingPlan_Creation_Works()
    {
        var mappingPlan = new DefaultMappingPlan();

        Assert.NotNull(mappingPlan);
    }

    [Fact]
    public void DefaultMappingPlan_IsValidByDefault()
    {
        var mappingPlan = new DefaultMappingPlan();

        Assert.True(mappingPlan.IsValid);
    }
}