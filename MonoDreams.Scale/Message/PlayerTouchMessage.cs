using DefaultEcs;
using MonoDreams.Scale.Util;

namespace MonoDreams.Scale.Message;

public class PlayerTouchMessage(Entity touchingEntity, TouchingSide side)
{
    public Entity TouchingEntity { get; } = touchingEntity;
    public TouchingSide Side { get; } = side;
}