using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDreams.Component;
using MonoDreams.Scale.Component;
using MonoDreams.Scale.Util;

namespace MonoDreams.Scale.Objects;

public static class Player
{
    public static Entity Create(World world, int gravity, Texture2D texture, Vector2 position, Enum? drawLayer = null)
    {
        var entity = world.CreateEntity();
        entity.Set(new PlayerState());
        entity.Set(new PlayerInput());
        entity.Set(new Position(position));
        entity.Set(new Collidable(new Rectangle(Point.Zero, Constants.PlayerSize), passive: false));
        entity.Set(new DrawInfo(texture, Constants.PlayerSize, layer: drawLayer));
        entity.Set(new MovementController());
        entity.Set(new DynamicBody(gravity));
        return entity;
    }
}
