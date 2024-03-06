using System;
using DefaultEcs;
using Microsoft.Xna.Framework;
using MonoDreams.Component;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace HeartfeltLending.Entities;

public static class Button
{
    public static Entity Create(
        World world, string value, Action callback, Vector2 position, Point size, BitmapFont font, Color? defaultColor = null, Color? selectedColor = null)
    {
        defaultColor ??= Color.White;
        selectedColor ??= Color.Black;
        var entity = world.CreateEntity();
        entity.Set(new Position(position));
        entity.Set(new Collidable(new Rectangle(size/new Point(-2, -2), size)));
        entity.Set(new Text(value, font, defaultColor.Value, HorizontalAlign.Center, VerticalAlign.Center));
        entity.Set(new ButtonState(defaultColor.Value, selectedColor.Value, callback));
        return entity;
    }
    
    public static Entity Create(World world, string value, Action callback, Vector2 position, BitmapFont font, Color? defaultColor = null, Color? selectedColor = null)
    {
        var size = font.MeasureString(value) + new Size2(5, 5);
        return Create(world, value, callback, position, new Point((int)size.Width, (int)size.Height), font, defaultColor, selectedColor);
    }
}