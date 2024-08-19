using DefaultEcs;
using DefaultEcs.System;
using MonoDreams.Component;
using MonoDreams.Message;
using MonoDreams.Scale.Component;
using MonoDreams.Scale.Objects;
using MonoDreams.Scale.Util;
using MonoDreams.State;

namespace MonoDreams.Scale.System.Collision;

public class ReactiveTileCollisionSystem : ISystem<GameState>
{
    private readonly List<CollisionMessage> _collisions;
    private readonly List<CollisionMessage> _lastTileCollisions;
    private readonly List<(CollisionMessage collision, TouchingSide? side)> _lastScaled;
    private const int ScalingSpeed = 35;
        
    public ReactiveTileCollisionSystem(World world)
    {
        world.Subscribe(this);
        _collisions = [];
        _lastTileCollisions = [];
        _lastScaled = [];
    }

    [Subscribe]
    private void On(in CollisionMessage message) => _collisions.Add(message);
        
    public bool IsEnabled { get; set; } = true;
        
    public void Dispose()
    {
        _collisions.Clear();
    }

    public void Update(GameState state)
    {
        _lastScaled.ForEach(scaled =>
        {
            var player = scaled.collision.BaseEntity;
            var playerPosition = player.Get<Position>();
            switch (scaled.side)
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
            }
        });
        _lastScaled.Clear();
        
        _lastTileCollisions.ForEach(collision =>
        {
            var tile = collision.CollidingEntity;
            var reactiveTile = tile.Get<ReactiveTile>();
            var drawInfo = tile.Get<DrawInfo>();
            reactiveTile.IsActivated = false;
            drawInfo.Color = Tile.DefaultReactiveColor;
        });
        _lastTileCollisions.Clear();
        
        foreach (var collision in _collisions.Where(collision => collision.CollidingEntity.Has<ReactiveTile>()))
        {
            var tile = collision.CollidingEntity;
            var player = collision.BaseEntity;
            var playerState = player.Get<PlayerState>();
            
            if (playerState.Grabbing.entity != tile) continue;
            var reactiveTile = tile.Get<ReactiveTile>();
            var drawInfo = tile.Get<DrawInfo>();
            reactiveTile.IsActivated = true;
            drawInfo.Color = Tile.ActiveReactiveColor;
            _lastTileCollisions.Add(collision);
            
            var playerInput = player.Get<PlayerInput>();
            if (!playerInput.Jump.Active) continue;
            var tilePosition = tile.Get<Position>();
            var playerPosition = player.Get<Position>();
            var collidable = tile.Get<Collidable>();
            switch (playerState.Grabbing.side)
            {
                case TouchingSide.Left:
                    tilePosition.NextLocation.X -= ScalingSpeed;
                    playerPosition.NextLocation.X = tilePosition.NextLocation.X - 22;
                    playerPosition.CurrentLocation.X = playerPosition.NextLocation.X;
                    collidable.Bounds.Width += ScalingSpeed;
                    break;
                case TouchingSide.Right:
                    playerPosition.NextLocation.X = tilePosition.NextLocation.X + 22;
                    playerPosition.CurrentLocation.X = playerPosition.NextLocation.X;
                    collidable.Bounds.Width += ScalingSpeed;
                    break;
                case TouchingSide.Top:
                    tilePosition.NextLocation.Y -= ScalingSpeed;
                    playerPosition.NextLocation.Y = tilePosition.NextLocation.Y - 22;
                    playerPosition.CurrentLocation.Y = playerPosition.NextLocation.Y;
                    collidable.Bounds.Height += ScalingSpeed;
                    break;
                default:
                    continue;
            }
            drawInfo.Size = collidable.Bounds.Size;
            _lastScaled.Add((collision, playerState.Grabbing.side));
        }

        _collisions.Clear();
    }
}