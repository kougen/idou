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
    public void MapsTheEntityType_CaseInsensitive_WhenTheMappingExists()
    {
        var plan = new JsonMappingPlan("Mapping/assets/valid.json");
        var sourceType = new EntityType("UsErS");
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

    [Fact]
    public void MapsTheEntityKey_WhenTheMappingExists()
    {
        var plan = new JsonMappingPlan("Mapping/assets/valid.json");
        var sourceType = new EntityType("users");
        var sourceKey = new EntityKey("u1");

        var mappedKey = plan.MapKey(sourceType, sourceKey);

        Assert.Equal(sourceKey.Value, mappedKey.Value);
    }

    [Fact]
    public void MapsTheEntityRecord_WhenTheMappingExists()
    {
        var plan = new JsonMappingPlan("Mapping/assets/valid.json");
        var sourceRecord = new EntityRecord
        {
            Type = new EntityType("users"),
            Key = new EntityKey("u1"),
            Attributes = new Dictionary<string, object?>
            {
                { "name", "Alice" }
            }
        };

        var mappedRecord = plan.MapRecord(sourceRecord);

        Assert.Equal("accounts", mappedRecord.Type.Name);
        Assert.Equal(sourceRecord.Key.Value, mappedRecord.Key.Value);
        Assert.Equal(sourceRecord.Attributes["name"], mappedRecord.Attributes["name"]);
    }
}