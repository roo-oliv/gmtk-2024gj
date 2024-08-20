using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDreams.Renderer;
using MonoDreams.Scale.Util;

namespace MonoDreams.Scale.Objects;

public static class LevelBoundaries
{
    public static void Create(World world, Texture2D square, ResolutionIndependentRenderer renderer)
    {
        Tile.Create(
            world, square,
            new Vector2(-renderer.VirtualWidth / 2, -renderer.VirtualHeight / 2 - 1000), new Point(renderer.VirtualWidth, 1000),
            TileType.Deadly,
            DrawLayer.Tiles);
        Tile.Create(
            world, square,
            new Vector2(-renderer.VirtualWidth / 2, renderer.VirtualHeight / 2), new Point(renderer.VirtualWidth, 1000),
            TileType.Deadly,
            DrawLayer.Tiles);
        Tile.Create(
            world, square,
            new Vector2(-renderer.VirtualWidth / 2 - 1000, -renderer.VirtualHeight / 2), new Point(1000, renderer.VirtualHeight),
            TileType.Deadly,
            DrawLayer.Tiles);
        Tile.Create(
            world, square,
            new Vector2(renderer.VirtualWidth / 2, -renderer.VirtualHeight / 2), new Point(1000, renderer.VirtualHeight),
            TileType.Deadly,
            DrawLayer.Tiles);
    }
}