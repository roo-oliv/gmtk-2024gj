using Microsoft.Xna.Framework;

namespace MonoDreams.Component;

public class Collidable(Rectangle bounds, bool passive = true)
{
    public bool Passive = passive;
    public Rectangle Bounds = bounds;
}