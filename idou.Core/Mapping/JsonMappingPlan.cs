using idou.Core.Domain;
using Newtonsoft.Json;

namespace idou.Core.Mapping;

public sealed class JsonMappingPlan : IMappingPlan
{
    private readonly IReadOnlyDictionary<string, JsonMapping> _bySource;

    public JsonMappingPlan(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Mapping file not found: {filePath}");

        var mappings = JsonConvert.DeserializeObject<List<JsonMapping>>(
            File.ReadAllText(filePath)
        ) ?? throw new InvalidOperationException("Invalid mapping file");

        _bySource = mappings.ToDictionary(m => m.Source, StringComparer.OrdinalIgnoreCase);
    }

    public bool IsValid => _bySource.Count > 0;

    public EntityType MapEntityType(EntityType sourceType)
    {
        return _bySource.TryGetValue(sourceType.Name, out var mapping)
            ? new EntityType(mapping.Target)
            : sourceType;
    }

    public EntityKey MapKey(EntityType sourceType, EntityKey sourceKey)
    {
        return sourceKey;
    }

    public EntityRecord MapRecord(EntityRecord sourceRecord)
    {
        var targetType = MapEntityType(sourceRecord.Type);
        var targetKey = MapKey(sourceRecord.Type, sourceRecord.Key);

        return new EntityRecord
        {
            Key = targetKey,
            Type = targetType,
            Attributes = new Dictionary<string, object?>(sourceRecord.Attributes)
        };
    }
}