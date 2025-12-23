using idou.Core.Domain;

namespace idou.Core.Mapping;

public class DefaultMappingPlan : IMappingPlan
{
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