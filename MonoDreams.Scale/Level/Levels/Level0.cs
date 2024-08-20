using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoDreams.Renderer;
using MonoDreams.Scale.Objects;
using MonoDreams.Scale.Util;

namespace MonoDreams.Scale.Level.Levels;

public class Level0(ContentManager content, ResolutionIndependentRenderer renderer) : ILevel
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
        Tile.Create(world, _square, new Vector2(-1000, 400), new Point(670, 100), TileType.Default, DrawLayer.Tiles);
        
        // small hill
        Tile.Create(world, _square, new Vector2(-700, 300), new Point(220, 200), TileType.Default, DrawLayer.Tiles);
        
        // left cliff
        Tile.Create(world, _square, new Vector2(-480, 200), new Point(150, 400), TileType.Default, DrawLayer.Tiles);
        
        // right cliff
        Tile.Create(world, _square, new Vector2(-130, 200), new Point(500, 520), TileType.Default, DrawLayer.Tiles);
        
        // big hill
        Tile.Create(world, _square, new Vector2(350, -300), new Point(150, 800), TileType.Default, DrawLayer.Tiles);
        
        // big upside hill
        Tile.Create(world, _square, new Vector2(-100, -550), new Point(300, 600), TileType.Default, DrawLayer.Tiles);
        
        // finish floor
        Tile.Create(world, _square, new Vector2(350, 400), new Point(700, 100), TileType.Default, DrawLayer.Tiles);
        
        // right wall
        Tile.Create(world, _square, new Vector2(800, -550), new Point(200, 850), TileType.Default, DrawLayer.Tiles);
        
        // objective
        Tile.Create(world, _square, new Vector2(950, 300), new Point(200, 150), TileType.Objective, DrawLayer.Tiles);
    }
}