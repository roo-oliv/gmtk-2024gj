using System;
using DefaultEcs;
using Microsoft.Xna.Framework;
using MonoDreams.Component;
using MonoGame.Extended.BitmapFonts;

namespace MonoDreams.Objects.UI;

public class StaticText
{
    public static Entity Create(World world, BitmapFont font, string value, Color color, Vector2 position, Enum drawLayer)
    {
        var entity = world.CreateEntity();
        entity.Set(new Position(position));
        entity.Set(new SimpleText(value, font, color, HorizontalAlign.Center, VerticalAlign.Center, drawLayer));
        return entity;
    }
}