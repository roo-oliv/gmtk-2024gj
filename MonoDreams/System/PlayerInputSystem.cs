using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Input;
using MonoDreams.Component;
using MonoDreams.State;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace MonoDreams.System;

public class PlayerInputSystem(World world) : AComponentSystem<GameState, PlayerInput>(world)
{
    protected override void Update(GameState state, ref PlayerInput playerInput)
    {
        playerInput.Jump.Update(Keyboard.GetState().IsKeyDown(Keys.Space));
        playerInput.Left.Update(Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A));
        playerInput.Right.Update(Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D));
        playerInput.Up.Update(Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W));
        playerInput.Down.Update(Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S));
        playerInput.Grab.Update(Keyboard.GetState().IsKeyDown(Keys.K));
        playerInput.CursorPosition = Mouse.GetState().Position.ToVector2();
        playerInput.LeftClick.Update( Mouse.GetState().LeftButton == ButtonState.Pressed);
    }
}