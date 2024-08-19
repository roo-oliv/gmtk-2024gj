using DefaultEcs;
using MonoDreams.Scale.Util;

namespace MonoDreams.Scale.Component;

public class PlayerState
{
    public MovementState Movement = MovementState.Idle;
    public (Entity? entity, TouchingSide? side) Grabbing = (null, null);
    public Entity? Riding = null;
}

public enum MovementState
{
    Idle,
    Running,
    Jumping,
    Falling,
    Climbing,
    Dying
}
