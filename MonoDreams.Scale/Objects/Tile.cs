using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDreams.Component;
using MonoDreams.Scale.Component;

namespace MonoDreams.Scale.Objects;

public class Tile
{
    public static Color DefaultColor = new(32, 40, 51);
    public static Color DefaultReactiveColor = new(32, 46, 51);
    public static Color ActiveReactiveColor = new(32, 51, 49);
    
    public static Entity Create(World world, Texture2D texture, Vector2 position, Point size, bool reactive = false, Enum? drawLayer = null)
    {
        var entity = world.CreateEntity();
        entity.Set(new Position(position));
        entity.Set(new Collidable( new Rectangle(Point.Zero, size)));
        entity.Set(new DrawInfo(texture, size, color: reactive ? DefaultReactiveColor : DefaultColor, layer: drawLayer));
        if (reactive)
        {
            entity.Set(new ReactiveTile());
        }
        return entity;
    }
}