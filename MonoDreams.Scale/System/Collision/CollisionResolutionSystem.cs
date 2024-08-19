using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using MonoDreams.Component;
using MonoDreams.Message;
using MonoDreams.Scale.Component;
using MonoDreams.Scale.Message;
using MonoDreams.Scale.Util;
using MonoDreams.State;

namespace MonoDreams.Scale.System.Collision;

public class CollisionResolutionSystem<TCollidable, TPosition, TDynamicBody> : ISystem<GameState>
    where TCollidable : Collidable
    where TPosition : Position
    where TDynamicBody : DynamicBody
{
    private readonly World _world;
    private readonly List<CollisionMessage> _collisions;
        
    public CollisionResolutionSystem(World world)
    {
        _world = world;
        world.Subscribe(this);
        _collisions = [];
    }

    [Subscribe]
    private void On(in CollisionMessage message) => _collisions.Add(message);
        
    public bool IsEnabled { get; set; } = true;
        
    public void Dispose()
    {
        _collisions.Clear();
    }

    public void Update(GameState state)
    {
        _collisions.Sort((l, r) => l.ContactTime.CompareTo(r.ContactTime));
        foreach (var collision in _collisions)
        {
            ResolveCollision(collision);
        }
        _collisions.Clear();
    }

    private void ResolveCollision(CollisionMessage collision)
    {
        var entity = collision.BaseEntity;
        var bounds = entity.Get<TCollidable>().Bounds;
        ref var position = ref entity.Get<TPosition>();
        var dynamicRect = new Rectangle(bounds.Location + position.CurrentLocation.ToPoint(), bounds.Size);
        ref var body = ref entity.Get<TDynamicBody>();
        var displacement = position.NextLocation - position.CurrentLocation;

        var collidingEntity = collision.CollidingEntity;
        var targetBounds = collidingEntity.Get<TCollidable>().Bounds;
        var targetPosition = collidingEntity.Get<TPosition>();
        var targetRect = new Rectangle(targetBounds.Location + targetPosition.CurrentLocation.ToPoint(), targetBounds.Size);
        if (!CollisionDetectionSystem.DynamicRectVsRect(dynamicRect, displacement, targetRect,
                out var contactPoint, out var contactNormal, out var contactTime)) return;
        if (contactNormal == Vector2.Zero || collidingEntity.Has<InstantDeath>())
        {
            _world.Publish(new DeathMessage());
            return;
        }
        
        if (contactNormal.X != 0)
        {
            position.NextLocation.X = contactPoint.X - dynamicRect.Width / 2f;
            position.CurrentLocation.X = position.NextLocation.X;
            
            if ((int)Math.Abs(contactPoint.X + dynamicRect.Width / 2f - targetRect.Left) == 0)
            {
                if (entity.Has<PlayerState>())
                {
                    _world.Publish(new PlayerTouchMessage(collidingEntity, TouchingSide.Left));
                }
            }
            else if ((int)Math.Abs(contactPoint.X - dynamicRect.Width / 2f - targetRect.Right) == 0)
            {
                if (entity.Has<PlayerState>())
                {
                    _world.Publish(new PlayerTouchMessage(collidingEntity, TouchingSide.Right));
                }
            }
        }

        if (contactNormal.Y != 0)
        {
            position.NextLocation.Y = contactPoint.Y - dynamicRect.Height / 2f;
            position.CurrentLocation.Y = position.NextLocation.Y;
            // body.IsRiding = false;
            if ((int)Math.Abs(contactPoint.Y + dynamicRect.Height / 2f - targetRect.Top) == 0)
            {
                // body.IsRiding = true;
                if (entity.Has<PlayerState>())
                {
                    _world.Publish(new PlayerTouchMessage(collidingEntity, TouchingSide.Top));
                }
            }
        }
    }
}