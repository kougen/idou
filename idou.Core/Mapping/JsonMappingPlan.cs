using idou.Core.Domain;
using Newtonsoft.Json;

namespace idou.Core.Mapping;

public class JsonMappingPlan : IMappingPlan
{
    private readonly IList<JsonMapping> _mappings;

    public JsonMappingPlan(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        _mappings = JsonConvert.DeserializeObject<IList<JsonMapping>>(File.ReadAllText(filePath))?.ToList() ?? [];
    }

    public bool IsValid => _mappings.Any();

    public EntityType MapEntityType(EntityType sourceType)
    {
        return sourceType;
    }

    public EntityKey MapKey(EntityType sourceType, EntityKey sourceKey)
    {
        return sourceKey;
    }

    public EntityRecord MapRecord(EntityRecord sourceRecord)
    {
        return sourceRecord;
    }
}