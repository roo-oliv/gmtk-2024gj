using DefaultEcs;
using DefaultEcs.System;
using MonoDreams.Component;
using MonoDreams.Message;
using MonoDreams.Scale.Component;
using MonoDreams.Scale.Objects;
using MonoDreams.Scale.Util;
using MonoDreams.State;

namespace MonoDreams.Scale.System.Collision;

public class ReactiveTileCollisionSystem : AEntitySetSystem<GameState>
{
    private readonly List<CollisionMessage> _collisions;
    private Entity? _lastActiveTile;
    private TouchingSide? _lastTouchingSide;
    private const int ScalingSpeed = 20;
        
    public ReactiveTileCollisionSystem(World world) : base(world.GetEntities().With<PlayerState>().AsSet())
    {
        _collisions = [];
    }
    
    public override void Dispose()
    {
        _collisions.Clear();
    }

    protected override void Update(GameState state, in Entity player)
    {
        var playerPosition = player.Get<Position>();
        switch (_lastTouchingSide)
        {
            case TouchingSide.Left:
                playerPosition.CurrentLocation.X = playerPosition.NextLocation.X + ScalingSpeed;
                break;
            case TouchingSide.Right:
                playerPosition.CurrentLocation.X = playerPosition.NextLocation.X - ScalingSpeed;
                break;
            case TouchingSide.Top:
                playerPosition.CurrentLocation.Y = playerPosition.NextLocation.Y + ScalingSpeed;
                break;
            case null:
                break;
        }
        _lastTouchingSide = null;
        
        if (_lastActiveTile != null) {
            _lastActiveTile.Value.Get<ReactiveTile>().IsActivated = false;
            _lastActiveTile.Value.Get<DrawInfo>().Color = Tile.DefaultReactiveColor;
        }
        _lastActiveTile = null;
        
        var playerState = player.Get<PlayerState>();
        if (playerState.Grabbing.entity == null || !playerState.Grabbing.entity.Value.Has<ReactiveTile>()) return;
        
        var tile = playerState.Grabbing.entity.Value;
        var reactiveTile = tile.Get<ReactiveTile>();
        var drawInfo = tile.Get<DrawInfo>();
        reactiveTile.IsActivated = true;
        drawInfo.Color = Tile.ActiveReactiveColor;
        _lastActiveTile = tile;
        var tilePosition = tile.Get<Position>();
        var collidable = tile.Get<Collidable>();
        switch (playerState.Grabbing.side)
        {
            case TouchingSide.Left:
                tilePosition.NextLocation.X -= ScalingSpeed;
                collidable.Bounds.Width += ScalingSpeed;
                playerPosition.NextLocation.X = tilePosition.NextLocation.X - 22;
                playerPosition.CurrentLocation.X = playerPosition.NextLocation.X;
                break;
            case TouchingSide.Right:
                collidable.Bounds.Width += ScalingSpeed;
                playerPosition.NextLocation.X = tilePosition.NextLocation.X + collidable.Bounds.Width;
                playerPosition.CurrentLocation.X = playerPosition.NextLocation.X;
                break;
            case TouchingSide.Top:
                tilePosition.NextLocation.Y -= ScalingSpeed;
                collidable.Bounds.Height += ScalingSpeed;
                playerPosition.NextLocation.Y = tilePosition.NextLocation.Y - 22;
                playerPosition.CurrentLocation.Y = playerPosition.NextLocation.Y;
                break;
        }
        drawInfo.Size = collidable.Bounds.Size;
        _lastTouchingSide = playerState.Grabbing.side;

        _collisions.Clear();
    }
}