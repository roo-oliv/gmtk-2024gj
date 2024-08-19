using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using MonoDreams.Component;
using MonoDreams.Scale.Component;
using MonoDreams.Scale.Message;
using MonoDreams.Scale.Util;
using MonoDreams.State;

namespace MonoDreams.Scale.System.Collision;

public sealed class RidingSystem : AEntitySetSystem<GameState>
{
    private readonly World _world;
    private readonly List<PlayerTouchMessage> _touches;
        
    public RidingSystem(World world) : base(world.GetEntities().With<PlayerState>().AsSet())
    {
        _world = world;
        world.Subscribe(this);
        _touches = [];
    }

    [Subscribe]
    private void On(in PlayerTouchMessage message) => _touches.Add(message);
    
    public override void Dispose()
    {
        _touches.Clear();
    }

    protected override void Update(GameState state, in Entity player)
    {
        var playerState = player.Get<PlayerState>();
        var playerInput = player.Get<PlayerInput>();
        var playerBody = player.Get<DynamicBody>();
        var playerMovementController = player.Get<MovementController>();
        if (!playerInput.Grab.Active)
        {
            playerState.Movement = playerBody.IsSliding ? MovementState.Falling : MovementState.Idle;
            playerState.Grabbing = (null, null);
            playerBody.IsRiding = false;
            playerBody.IsSliding = false;
        }

        foreach (var touch in _touches)
        {
            if (playerInput.Grab.Active)
            {
                playerState.Grabbing = (touch.TouchingEntity, touch.Side);
                playerBody.IsRiding = true;
                playerState.Riding = touch.TouchingEntity;
                playerMovementController.Velocity = Vector2.Zero;
            }

            switch (touch.Side)
            {
                case TouchingSide.Top:
                    playerBody.IsRiding = true;
                    playerState.Riding = touch.TouchingEntity;
                    break;
                case TouchingSide.Left or TouchingSide.Right:
                {
                    playerBody.IsRiding = true;
                    playerState.Riding = touch.TouchingEntity;
                    playerBody.IsSliding = true;
                    playerBody.SlidingSide = touch.Side;
                    if (playerState.Grabbing.entity != null)
                    {
                        playerState.Movement = MovementState.Climbing;
                    }
                    break;
                }
            }
        }
        _touches.Clear();
    }
}