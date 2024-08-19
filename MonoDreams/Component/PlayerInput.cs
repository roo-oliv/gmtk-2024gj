using Microsoft.Xna.Framework;

namespace MonoDreams.Component;

public class PlayerInput
{
    public readonly InputState.InputState Jump;
    public readonly InputState.InputState Right;
    public readonly InputState.InputState Left;
    public readonly InputState.InputState Up;
    public readonly InputState.InputState Down;
    public readonly InputState.InputState Grab;
    public Vector2 CursorPosition;
    public readonly InputState.InputState LeftClick;
        
    public PlayerInput()
    {
        Jump = new InputState.InputState();
        Right = new InputState.InputState();
        Left = new InputState.InputState();
        Up = new InputState.InputState();
        Down = new InputState.InputState();
        Grab = new InputState.InputState();
        CursorPosition = Vector2.Zero;
        LeftClick = new InputState.InputState();
    }
}