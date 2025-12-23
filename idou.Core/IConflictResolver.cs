using idou.Core.Domain;

namespace idou.Core;

public interface IConflictResolver
{
    ChangeEvent Resolve(ChangeEvent incoming, EntityRecord? currentTarget);
}