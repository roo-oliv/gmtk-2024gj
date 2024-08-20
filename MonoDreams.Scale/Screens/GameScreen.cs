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
using MonoDreams.Scale.Component;
using MonoDreams.Scale.Level;
using MonoDreams.Scale.Objects;
using MonoDreams.Scale.System;
using MonoDreams.Scale.System.Collision;
// using MonoDreams.Scale.System.Debug;
using MonoDreams.Scale.Util;
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

    public LevelLoader LevelLoader { get; set; }
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
        LevelLoader = new LevelLoader(World, content, renderer);
        System = CreateSystem();
    }

    private SequentialSystem<GameState> CreateSystem()
    {
        return new SequentialSystem<GameState>(
            // new DebugSystem(World, _game, _spriteBatch),
            new PlayerInputSystem(World),
            new PlayerMovementSystem<DynamicBody, PlayerInput, MovementController>(WorldGravity, World,
                _parallelRunner),
            new DynamicBodySystem<DynamicBody, MovementController, Position, PlayerInput>(WorldGravity, World,
                _parallelRunner),
            new CollisionDetectionSystem(World, _parallelRunner),
            new CollisionResolutionSystem<Collidable, Position, DynamicBody>(World),
            new RemainderCollisionDetectionSystem(World, _parallelRunner),
            new RidingSystem(World),
            new ReactiveTileCollisionSystem(World),
            new DeathSystem(World, () =>
            {
                ResetWorld();
                LevelLoader.ReloadLevel(World);
            }),
            new LayoutSystem(World, _renderer),
            new SizeSystem(World, _parallelRunner),
            new PositionSystem<Position>(World, _parallelRunner),
            new BeginDrawSystem(_spriteBatch, _renderer, _camera),
            new DrawSystem(World, _spriteBatch),
            new CompositeDrawSystem(_spriteBatch, World),
            new TextSystem(_spriteBatch, World),
            new EndDrawSystem(_spriteBatch),
            new FinishLevelSystem(World, () =>
            {
                ResetWorld();
                LevelLoader.LoadNextLevel(World);
            }));
        // new DrawDebugSystem(World, _spriteBatch, _renderer));
    }

    public void Load(ScreenController screenController, ContentManager content)
    {
        // LoadContent(content);
        // LoadLevel();
        LevelLoader.LoadLevel(0);
    }
    
    // public void LoadContent(ContentManager content)
    // {
    //     // var backgroundImage = content.Load<Texture2D>("buttons/Small Square Buttons");
    //     // StaticBackground.Create(World, backgroundImage, _camera, _renderer, drawLayer: DrawLayer.Background);
    //     square = content.Load<Texture2D>("square");
    // }

    // public void LoadLevel()
    // {
    //     Debug.Create(World, _spriteBatch, _renderer);
    //     var player = Player.Create(World, WorldGravity, square, new Vector2(-900, 370), DrawLayer.Player);
    // }

    public void Dispose()
    {
        World.Dispose();
        System.Dispose();
        GC.SuppressFinalize(this);
    }

    public void ResetWorld()
    {
        World.Dispose();
        System.Dispose();
        World = new World();
        System = CreateSystem();
    }
}
