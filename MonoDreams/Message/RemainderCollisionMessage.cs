using DefaultEcs;
using Microsoft.Xna.Framework;

namespace MonoDreams.Message;

public readonly record struct RemainderCollisionMessage(
    Entity BaseEntity,
    Entity CollidingEntity,
    Vector2 ContactPoint,
    Vector2 ContactNormal,
    float ContactTime);
