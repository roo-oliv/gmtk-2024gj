namespace MonoDreams.Component;

public class DynamicBody(int gravity)
{
    public bool IsRiding = false;
    public bool IsSliding = false;
    public bool IsJumping = false;
    public int Gravity = gravity;
}