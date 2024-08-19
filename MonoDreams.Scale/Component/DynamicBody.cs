using MonoDreams.Scale.Util;

namespace MonoDreams.Scale.Component;

public class DynamicBody(int gravity)
{
    private const float GracePeriod = 0.12f;
    private bool _isRiding = false;
    public float WasRidingGracePeriod = 0;
    public bool IsRiding
    {
        get => _isRiding;
        set
        {
            if (value) WasRidingGracePeriod = 0;
            else if (_isRiding && value is false) WasRidingGracePeriod = GracePeriod;
            _isRiding = value;
        }
    }

    private bool _isSliding = false;
    public float WasSlidingGracePeriod = 0;

    public bool IsSliding
    {
        get => _isSliding;
        set
        {
            if (value) WasSlidingGracePeriod = 0;
            else if (_isSliding && value is false) WasSlidingGracePeriod = GracePeriod;
            _isSliding = value;
        }
    }

    public TouchingSide? SlidingSide;
    public bool IsJumping = false;
    public int Gravity = gravity;
}