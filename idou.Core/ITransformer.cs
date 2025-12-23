using idou.Core.Domain;

namespace idou.Core;

public interface ITransformer
{
    ChangeEvent Transform(ChangeEvent change);
}