using Microsoft.Xna.Framework;

namespace MonoDreams.Scale.Util;

public static class Constants
{
    public static int WorldGravity => 1600;
    public static int JumpGravity => 1250;
    public static int JumpVelocity => -400;
    public static int JumpHVelocity => 300;
    public static int MaxWalkVelocity => 210;
    public static int MaxFallVelocity => 700;
    public static int SlidingVelocity => 12;
    
    public static int ScalingSpeed => 4;
    public static Point PlayerSize { get; } = new(10);
}