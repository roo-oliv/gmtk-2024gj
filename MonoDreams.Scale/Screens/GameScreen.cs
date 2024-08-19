using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Facebook.Yoga;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoDreams.Component;
using MonoDreams.Extensions;
using MonoDreams.Objects.UI;
using MonoDreams.Renderer;
using MonoDreams.Scale.Objects;
using MonoDreams.Scale.System;
using MonoDreams.Scale.System.Collision;
using MonoDreams.Scale.System.Debug;
using MonoDreams.Screen;
using MonoDreams.State;
using MonoDreams.System;
using MonoDreams.System.Draw;
using MonoGame.Extended.BitmapFonts;

namespace MonoDreams.Scale.Screens;

public class GameScreen : IGameScreen
{
    private readonly Game _game;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly Camera _camera;
    private readonly ResolutionIndependentRenderer _renderer;
    private readonly DefaultParallelRunner _parallelRunner;
    private readonly SpriteBatch _spriteBatch;
    public World World { get; set; }
    public ISystem<GameState> System { get; set; }
    public int WorldGravity = 5000;
    private Texture2D square;

    public GameScreen(Game game, GraphicsDevice graphicsDevice, ContentManager content, Camera camera,
        ResolutionIndependentRenderer renderer, DefaultParallelRunner parallelRunner, SpriteBatch spriteBatch)
    {
        _game = game;
        _graphicsDevice = graphicsDevice;
        _camera = camera;
        _renderer = renderer;
        _parallelRunner = parallelRunner;
        _spriteBatch = spriteBatch;
        
        World = new World();
        System = CreateSystem();
    }

    private SequentialSystem<GameState> CreateSystem()
    {
        return new SequentialSystem<GameState>(
            new DebugSystem(World, _game, _spriteBatch),
            new PlayerInputSystem(World),
            new PlayerMovementSystem<DynamicBody, PlayerInput, MovementController>(WorldGravity, World, _parallelRunner),
            new DynamicBodySystem<DynamicBody, MovementController, Position, PlayerInput>(WorldGravity, World, _parallelRunner),
            new CollisionDetectionSystem(World, _parallelRunner),
            new CollisionResolutionSystem<Collidable, Position, DynamicBody>(World),
            new RemainderCollisionDetectionSystem(World, _parallelRunner),
            new RidingSystem(World),
            new ReactiveTileCollisionSystem(World),
            new DeathSystem(World, Reload),
            new LayoutSystem(World, _renderer),
            new SizeSystem(World, _parallelRunner),
            new PositionSystem<Position>(World, _parallelRunner),
            new BeginDrawSystem(_spriteBatch, _renderer, _camera),
            new DrawSystem(World, _spriteBatch),
            new CompositeDrawSystem(_spriteBatch, World),
            new TextSystem(_spriteBatch, World),
            new EndDrawSystem(_spriteBatch),
            new DrawDebugSystem(World, _spriteBatch, _renderer));
    }

    public void Load(ScreenController screenController, ContentManager content)
    {
        LoadContent(content);
        LoadLevel();
    }
    
    public void LoadContent(ContentManager content)
    {
        // var backgroundImage = content.Load<Texture2D>("buttons/Small Square Buttons");
        // StaticBackground.Create(World, backgroundImage, _camera, _renderer, drawLayer: DrawLayer.Background);
        square = content.Load<Texture2D>("square");
    }

    public void LoadLevel()
    {
        Debug.Create(World, _spriteBatch, _renderer);
        var player = Player.Create(World, WorldGravity, square, new Vector2(-900, 370), DrawLayer.Player);
        LoadTiles(square);
        LoadLevelBounds();
    }

    private void LoadLevelBounds()
    {
        LevelBoundary.Create(World, new Vector2(-_renderer.VirtualWidth / 2, -_renderer.VirtualHeight / 2 - 1000), new Point(_renderer.VirtualWidth, 1000));
        LevelBoundary.Create(World, new Vector2(-_renderer.VirtualWidth / 2, _renderer.VirtualHeight / 2), new Point(_renderer.VirtualWidth, 1000));
        LevelBoundary.Create(World, new Vector2(-_renderer.VirtualWidth / 2 - 1000, -_renderer.VirtualHeight / 2), new Point(1000, _renderer.VirtualHeight));
        LevelBoundary.Create(World, new Vector2(_renderer.VirtualWidth / 2, -_renderer.VirtualHeight / 2), new Point(1000, _renderer.VirtualHeight));
    }

    private void LoadTiles(Texture2D texture)
    {
        // floor
        // for (var i = 0; i < 37; ++i) Tile.Create(World, texture, new Vector2(i*51 - 950, 400), new Point(50), true, DrawLayer.Tiles);
        
        // ceiling
        Tile.Create(World, texture, new Vector2(-1000, -550), new Point(2000, 50), false, DrawLayer.Tiles);
        
        // left wall
        Tile.Create(World, texture, new Vector2(-1000, -500), new Point(70, 900), false, DrawLayer.Tiles);
        
        // initial floor
        Tile.Create(World, texture, new Vector2(-1000, 400), new Point(400, 100), false, DrawLayer.Tiles);
        
        // first platform
        Tile.Create(World, texture, new Vector2(-600, 400), new Point(80, 100), true, DrawLayer.Tiles);
        
        // first stopper
        Tile.Create(World, texture, new Vector2(-600, -550), new Point(80, 400), false, DrawLayer.Tiles);
        
        // second platform
        Tile.Create(World, texture, new Vector2(-400, -250), new Point(80, 180), true, DrawLayer.Tiles);
        
        // right wall
        Tile.Create(World, texture, new Vector2(800, -550), new Point(200, 500), false, DrawLayer.Tiles);
        
        // finish floor
        Tile.Create(World, texture, new Vector2(600, 100), new Point(400, 400), false, DrawLayer.Tiles);
    }

    public void Dispose()
    {
        World.Dispose();
        System.Dispose();
        GC.SuppressFinalize(this);
    }

    public void Reload()
    {
        Dispose();
        World = new World();
        System = CreateSystem();
        LoadLevel();
    }
    
    public enum DrawLayer
    {
        Cursor,
        Buttons,
        Player,
        Tiles,
        Background,
    }
}
