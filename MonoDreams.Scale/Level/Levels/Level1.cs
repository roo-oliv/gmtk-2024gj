using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoDreams.Renderer;
using MonoDreams.Scale.Objects;
using MonoDreams.Scale.Util;

namespace MonoDreams.Scale.Level.Levels;

public class Level1(ContentManager content, ResolutionIndependentRenderer renderer) : ILevel
{
    private readonly Texture2D _square = content.Load<Texture2D>("square");

    public void Load(World world)
    {
        Player.Create(world, Constants.WorldGravity, _square, new Vector2(-900, 370), DrawLayer.Player);
        LoadTiles(world);
        LevelBoundaries.Create(world, _square, renderer);
    }
    
    private void LoadTiles(World world)
    {
        // ceiling
        Tile.Create(world, _square, new Vector2(-1000, -550), new Point(2000, 50), TileType.Default, DrawLayer.Tiles);
        
        // left wall
        Tile.Create(world, _square, new Vector2(-1000, -500), new Point(70, 900), TileType.Default, DrawLayer.Tiles);
        
        // initial floor
        Tile.Create(world, _square, new Vector2(-1000, 400), new Point(400, 100), TileType.Default, DrawLayer.Tiles);
        
        // first platform
        Tile.Create(world, _square, new Vector2(-600, 400), new Point(80, 100), TileType.Reactive, DrawLayer.Tiles);
        
        // first stopper
        Tile.Create(world, _square, new Vector2(-600, -550), new Point(80, 400), TileType.Default, DrawLayer.Tiles);
        
        // second platform
        Tile.Create(world, _square, new Vector2(-400, -250), new Point(80, 180), TileType.Reactive, DrawLayer.Tiles);
        
        // middle obstacle
        Tile.Create(world, _square, new Vector2(-400, -50), new Point(1000, 80), TileType.Deadly, DrawLayer.Tiles);
        
        // right wall
        Tile.Create(world, _square, new Vector2(800, -550), new Point(200, 500), TileType.Default, DrawLayer.Tiles);
        
        // finish floor
        Tile.Create(world, _square, new Vector2(600, 100), new Point(400, 400), TileType.Default, DrawLayer.Tiles);
        
        // finish obstacle
        Tile.Create(world, _square, new Vector2(590, 105), new Point(10, 400), TileType.Deadly, DrawLayer.Tiles);
        
        // objective
        Tile.Create(world, _square, new Vector2(950, -50), new Point(200, 150), TileType.Objective, DrawLayer.Tiles);
    }
}