using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using MonoDreams.Component;
using MonoDreams.Message;
using MonoDreams.State;

namespace MonoDreams.System;

public sealed class PositionSystem<TPosition>(World world, IParallelRunner runner)
    : AEntitySetSystem<GameState>(world.GetEntities().With<TPosition>().AsSet(), runner)
    where TPosition : Position
{
    protected override void Update(GameState state, in Entity entity)
    {
        ref var position = ref entity.Get<TPosition>();
        if (!position.HasUpdates) return;
        position.LastLocation.X = position.CurrentLocation.X;
        position.LastLocation.Y = position.CurrentLocation.Y;
        position.CurrentLocation.X = position.NextLocation.X;
        position.CurrentLocation.Y = position.NextLocation.Y;
        position.LastOrientation = position.CurrentOrientation;
        position.CurrentOrientation = position.NextOrientation;
        world.Publish(new PositionChangeMessage(entity));
    }
}
