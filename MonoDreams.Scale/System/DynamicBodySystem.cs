using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using MonoDreams.Component;
using MonoDreams.Scale.Component;
using MonoDreams.Scale.Util;
using MonoDreams.State;

namespace MonoDreams.Scale.System;

public class DynamicBodySystem<TDynamicBody, TMovementController, TPosition, TPlayerInput>(int worldGravity, World world, IParallelRunner runner)
    : AEntitySetSystem<GameState>(world.GetEntities().With<TDynamicBody>().AsSet(), runner)
    where TDynamicBody : DynamicBody
    where TMovementController : MovementController
    where TPosition : Position
    where TPlayerInput : PlayerInput
{
    protected override void Update(GameState state, in Entity entity)
    {
        var dynamicBody = entity.Get<TDynamicBody>();
        if (dynamicBody.WasRidingGracePeriod > 0) dynamicBody.WasRidingGracePeriod -= state.Time;
        if (dynamicBody.WasSlidingGracePeriod > 0) dynamicBody.WasSlidingGracePeriod -= state.Time;
        var position = entity.Get<TPosition>();
        var movementController = entity.Get<TMovementController>();
        // var playerInput = entity.Get<TPlayerInput>();
        if (movementController is not null) ResolveMovement(position, movementController, state);
        // if (playerInput is not null && dynamicBody.IsRiding && playerInput.Grab.Active) return;
        var playerState = entity.Get<PlayerState>();
        if (dynamicBody.IsSliding && playerState?.Movement != MovementState.Jumping)
        {
            if (playerState?.Grabbing.entity is not null) return;
            position.NextLocation.Y = position.CurrentLocation.Y + Constants.SlidingVelocity * state.Time;
        }
        else
        {
            ResolveGravity(position, dynamicBody, playerState, state);
        }
    }

    private static void ResolveMovement(TPosition position, TMovementController movement, in GameState state)
    {
        if (movement.FreezeHVelocity.time > 0)
        {
            movement.FreezeHVelocity.time -= state.Time;
            movement.Velocity.X = movement.FreezeHVelocity.velocity;
        }
        position.NextLocation = position.CurrentLocation + movement.Velocity * state.Time;  // S_1 = S_0 + V * t
        movement.Clear();
    }

    private void ResolveGravity(TPosition position, TDynamicBody body, PlayerState? playerState, in GameState state)
    {
        var lastYVelocity = 0f;
        if (state.LastTime != 0)
        {
            lastYVelocity = (position.CurrentLocation.Y - position.LastLocation.Y) / state.LastTime;   // V = (S_1 - S_0) / t
        }

        var yVelocity = 0f;
        if (state.Time != 0)
        {
            yVelocity = (position.NextLocation.Y - position.CurrentLocation.Y) / state.Time; // V = (S_1 - S_0) / t
        }

        var gravityVelocity = lastYVelocity + body.Gravity * state.Time;  // V_1 = V_0 + a * t
        yVelocity = Math.Min(yVelocity + gravityVelocity, Constants.MaxFallVelocity);
        position.NextLocation.Y = position.CurrentLocation.Y + yVelocity * state.Time;  // S_1 = S_0 + V * t
        if (!body.IsJumping && body.Gravity != Constants.WorldGravity)
        {
            body.Gravity = Constants.WorldGravity;
        }
        else if (body.IsJumping && yVelocity > 0)
        {
            body.IsJumping = false;
            body.Gravity = Constants.WorldGravity;
            if (playerState is not null) playerState.Movement = MovementState.Falling;
        }
    }
}