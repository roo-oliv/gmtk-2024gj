using Microsoft.Xna.Framework;

namespace MonoDreams.Component;

public class MovementController
{
    public Vector2 Velocity;
    public (float time, float velocity) FreezeHVelocity = (0, 0);

    public MovementController()
    {
        Velocity = Vector2.Zero;
    }

    public void Clear()
    {
        Velocity = Vector2.Zero;
    }
}