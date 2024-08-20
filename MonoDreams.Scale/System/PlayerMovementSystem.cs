using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using MonoDreams.Component;
using MonoDreams.Scale.Component;
using MonoDreams.Scale.Util;
using MonoDreams.State;

namespace MonoDreams.Scale.System;

public class PlayerMovementSystem<TDynamicBody, TPlayerInput, TMovementController>(
    int worldGravity,
    World world,
    IParallelRunner runner)
    : AEntitySetSystem<GameState>(world.GetEntities().With<TDynamicBody>().With<TPlayerInput>().AsSet(), runner)
    where TDynamicBody : DynamicBody
    where TPlayerInput : PlayerInput
    where TMovementController : MovementController
{
    protected override void Update(GameState state, in Entity entity)
    {
        var position = entity.Get<Position>();
        var dynamicBody = entity.Get<TDynamicBody>();
        var playerInput = entity.Get<TPlayerInput>();
        var movementController = entity.Get<TMovementController>();

        if (playerInput.Left.JustActivated) position.NextOrientation = Orientation.Left;
        else if (playerInput.Right.JustActivated) position.NextOrientation = Orientation.Right;
        else if (playerInput.Up.JustActivated) position.NextOrientation = Orientation.Up;
        else if (playerInput.Down.JustActivated) position.NextOrientation = Orientation.Down;

        if (playerInput.Left.Active)
        {
            if (position.CurrentOrientation is Orientation.Right) position.NextOrientation = Orientation.Left;
            else if (position.LastOrientation is Orientation.Left) movementController.Velocity.X -= Constants.MaxWalkVelocity;
        }

        if (playerInput.Right.Active)
        {
            if (position.CurrentOrientation is Orientation.Left) position.NextOrientation = Orientation.Right;
            else if (position.LastOrientation is Orientation.Right) movementController.Velocity.X += Constants.MaxWalkVelocity;
        }

        if ((dynamicBody.IsRiding || dynamicBody.WasRidingGracePeriod > 0) && playerInput.Jump.JustActivated)
        {
            dynamicBody.Gravity = Constants.JumpGravity;
            if (dynamicBody.IsSliding || dynamicBody.WasSlidingGracePeriod > 0)
            {
                movementController.Velocity.X = dynamicBody.SlidingSide == TouchingSide.Left ? -Constants.JumpHVelocity : Constants.JumpHVelocity;
                movementController.FreezeHVelocity = (0.19f, movementController.Velocity.X);
                dynamicBody.Gravity = Constants.WorldGravity;
            }

            var playerState = entity.Get<PlayerState>();
            movementController.Velocity.Y = Constants.JumpVelocity;
            dynamicBody.IsJumping = true;
            dynamicBody.IsRiding = false;
            dynamicBody.IsSliding = false;
            playerState.Movement = MovementState.Jumping;
            playerState.Grabbing = (null, null);
            playerState.Riding = null;
        }
        else if (dynamicBody.IsJumping && !playerInput.Jump.Active)
        {
            var playerState = entity.Get<PlayerState>();
            playerState.Movement = MovementState.Falling;
            dynamicBody.IsJumping = false;
            dynamicBody.Gravity = Constants.WorldGravity;
        }
    }
}