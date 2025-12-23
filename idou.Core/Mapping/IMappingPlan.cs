using idou.Core.Domain;

namespace idou.Core.Mapping;

public interface IMappingPlan
{
    EntityType MapEntityType(EntityType sourceType);
    EntityKey MapKey(EntityType sourceType, EntityKey sourceKey);
    EntityRecord MapRecord(EntityRecord sourceRecord);
}