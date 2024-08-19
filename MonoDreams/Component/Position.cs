using Microsoft.Xna.Framework;

namespace MonoDreams.Component;

public class Position(Vector2 startingLocation, Orientation startingOrientation = Orientation.Right)
{
    public Vector2 CurrentLocation = startingLocation;
    public Vector2 NextLocation = startingLocation;
    public Vector2 LastLocation = startingLocation;
    public Orientation CurrentOrientation = startingOrientation;
    public Orientation NextOrientation = startingOrientation;
    public Orientation LastOrientation = startingOrientation;

    public bool HasUpdates => CurrentLocation != NextLocation || LastLocation != CurrentLocation || CurrentOrientation != NextOrientation || LastOrientation != CurrentOrientation;
}

public enum Orientation
{
    Up = -1,
    Down = 1,
    Left = -1,
    Right = 1
}
