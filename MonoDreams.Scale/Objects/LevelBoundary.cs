using DefaultEcs;
using Microsoft.Xna.Framework;
using MonoDreams.Component;
using MonoDreams.Scale.Component;

namespace MonoDreams.Scale.Objects;

public class LevelBoundary
{
    public static Entity Create(World world, Vector2 position, Point size)
    {
        var entity = world.CreateEntity();
        entity.Set(new Position(position));
        entity.Set(new Collidable( new Rectangle(Point.Zero, size)));
        entity.Set(new InstantDeath());
        return entity;
    }
}