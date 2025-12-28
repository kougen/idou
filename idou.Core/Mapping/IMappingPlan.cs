using idou.Core.Domain;

namespace idou.Core.Mapping;

public interface IMappingPlan
{
    bool IsValid { get; }
    EntityType MapEntityType(EntityType sourceType);
    EntityKey MapKey(EntityType sourceType, EntityKey sourceKey);
    EntityRecord MapRecord(EntityRecord sourceRecord);
}