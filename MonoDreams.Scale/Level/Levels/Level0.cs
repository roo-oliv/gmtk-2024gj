using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoDreams.Objects.UI;
using MonoDreams.Renderer;
using MonoDreams.Scale.Objects;
using MonoDreams.Scale.Util;
using MonoGame.Extended.BitmapFonts;

namespace MonoDreams.Scale.Level.Levels;

public class Level0(ContentManager content, ResolutionIndependentRenderer renderer) : ILevel
{
    private readonly Texture2D _square = content.Load<Texture2D>("square");
    private readonly BitmapFont font = content.Load<BitmapFont>("Fonts/pixel-8px-white-fnt");

    public const string Layout = @"
    .*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*.
    .*..*..*..@..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*.
    .*..*..W........W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W........W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W........W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W........W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W........W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W........W..W..W..W..W..W..W.....................................................W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W........W..W..W..W..W..W..W.....................................................W..W..W..W..........................W..*..*.
    .*..*..W..............W..W..W..W..W.....................................................W..W..W..W..........................W..*..*.
    .*..*..W..............W..W..W..W..W.....................................................W..W..W..W..........................W..*..*.
    .*..*..W..............W..W..W..W..W.....................................................W..W..W..W..........................W..*..*.
    .*..*..W..............W..W..W..W..W.....................................................W..W..W..W..........................O..*..*.
    .*..*..W..............W..W..W...........................................................W..W..W..W..........................O..*..*.
    .*..*..W..............W..W..W...........................................................W..W..W..W.................W..W..W..W..*..*.
    .*..*..W..............W..W..W.................................................................W..W.................W..W..W..W..*..*.
    .*..*..W..............W..W..W.................................................................W..W..W..W..............W..W..W..*..*.
    .*..*..W..............W..W..W.................................................................W..W..W.......................W..*..*.
    .*..*..W..............W..W..W....................W..W....................W..W..W..............W..W..W..W..W.................W..*..*.
    .*..*..W.........................................W..W....................W..W..W............................................W..*..*.
    .*..*..W...................................W..W..W..W....................W..W..W...................................W..W..W..W..*..*.
    .*..*..W...................................W..W..W..W....................W..W..W..W................................W..W..W..W..*..*.
    .*..*..W..........................W..W..W..W..W..W..W....................W..W..W..W.................W..W..W..............W..W..*..*.
    .*..*..W...........W..W..W..W..W..W..W..W..W..W..W..W....................W..W..W..W..W..W..W........W..W..W..............W..W..*..*.
    .*..*..W...........W..W..W..W..W..W..W..W..W..W..W..W....................W..W..W..W..W..W..W.............................W..W..*..*.
    .*..*..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W....................W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W....................W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W....................W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W....................W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W....................W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..W..*..*.
    .*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*.
    .*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*..*.
    ";

    public void Load(World world)
    {
        var levelLayout = string.Concat(Layout.Where(c => !char.IsWhiteSpace(c)));
        
        int columns = 44;
        int rows = 32;
        
        for (int i = 0; i < levelLayout.Length; i += 3)
        {
            int row = 20 * (i / (columns * 3));
            int col = 20 * ((i / 3) % columns);
            string chunk = levelLayout.Substring(i, 3);

            switch (chunk)
            {
                case "...":
                    break;
                case ".@.":
                    Player.Create(world, Constants.WorldGravity, _square, new Vector2(col+7, row+7), DrawLayer.Player);
                    break;
                case ".*.":
                    Tile.Create(
                        world, _square,
                        new Vector2(col, row), new Point(20, 20),
                        TileType.Deadly,
                        DrawLayer.Tiles);
                    break;
                case ".W.":
                    Tile.Create(
                        world, _square,
                        new Vector2(col, row), new Point(20, 20),
                        TileType.Default,
                        DrawLayer.Tiles);
                    break;
                case ".O.":
                    Tile.Create(
                        world, _square,
                        new Vector2(col, row), new Point(20, 20),
                        TileType.Objective,
                        DrawLayer.Tiles);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chunk), chunk, null);
            }
            
            StaticText.Create(world, font, "Move with Arrow Keys or A and D", Color.White, new Vector2(20 * 12, 20 * 24), DrawLayer.Text);
            StaticText.Create(world, font, "Jump with Spacebar", Color.White, new Vector2(20 * 12, 20 * 25), DrawLayer.Text);
            StaticText.Create(world, font, "Hold Jump to go farther", Color.White, new Vector2(20 * 18, 20 * 10), DrawLayer.Text);
        }
        

        // Player.Create(world, Constants.WorldGravity, _square, new Vector2(-900, 370), DrawLayer.Player);
        // LoadTiles(world);
        // LevelBoundaries.Create(world, _square, renderer);
    }
    
    private void LoadTiles(World world)
    {
        float factor = 0.3f;
        // ceiling
        Tile.Create(world, _square, new Vector2(-1000 * factor, -550 * factor), new Point((int)(2000 * factor), (int)(50 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // left wall
        Tile.Create(world, _square, new Vector2(-1000 * factor, -500 * factor), new Point((int)(70 * factor), (int)(900 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // initial floor
        Tile.Create(world, _square, new Vector2(-1000 * factor, 400 * factor), new Point((int)(670 * factor), (int)(100 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // small hill
        Tile.Create(world, _square, new Vector2(-700 * factor, 300 * factor), new Point((int)(220 * factor), (int)(200 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // left cliff
        Tile.Create(world, _square, new Vector2(-480 * factor, 200 * factor), new Point((int)(150 * factor), (int)(400 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // right cliff
        Tile.Create(world, _square, new Vector2(-130 * factor, 200 * factor), new Point((int)(500 * factor), (int)(520 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // big hill
        Tile.Create(world, _square, new Vector2(350 * factor, -300 * factor), new Point((int)(150 * factor), (int)(800 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // big upside hill
        Tile.Create(world, _square, new Vector2(-100 * factor, -550 * factor), new Point((int)(300 * factor), (int)(600 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // finish floor
        Tile.Create(world, _square, new Vector2(350 * factor, 400 * factor), new Point((int)(700 * factor), (int)(100 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // right wall
        Tile.Create(world, _square, new Vector2(800 * factor, -550 * factor), new Point((int)(200 * factor), (int)(850 * factor)), TileType.Default, DrawLayer.Tiles);
        
        // objective
        Tile.Create(world, _square, new Vector2(950 * factor, 300 * factor), new Point((int)(200 * factor), (int)(150 * factor)), TileType.Objective, DrawLayer.Tiles);
    }
}