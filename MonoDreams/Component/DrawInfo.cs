using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoDreams.Component;

public class DrawInfo(Texture2D spriteSheet, Point? size = null, Rectangle? source = null, Color? color = null, Enum layer = null, NinePatchInfo? ninePatchInfo = null)
{
    public Texture2D SpriteSheet = spriteSheet;
    public Point Size = size ?? spriteSheet.Bounds.Size;
    public Rectangle Source = source ?? spriteSheet.Bounds;
    public Color Color = color ?? Color.White;
    public Enum Layer = layer;
    public NinePatchInfo? NinePatchInfo = ninePatchInfo;
}

public readonly struct NinePatchInfo(
    Rectangle topLeft, Rectangle top, Rectangle topRight,
    Rectangle left, Rectangle center, Rectangle right,
    Rectangle bottomLeft, Rectangle bottom, Rectangle bottomRight)
{
    public Rectangle TopLeft { get; } = topLeft;
    public Rectangle Top { get; } = top;
    public Rectangle TopRight { get; } = topRight;
    public Rectangle Left { get; } = left;
    public Rectangle Center { get; } = center;
    public Rectangle Right { get; } = right;
    public Rectangle BottomLeft { get; } = bottomLeft;
    public Rectangle Bottom { get; } = bottom;
    public Rectangle BottomRight { get; } = bottomRight;
}