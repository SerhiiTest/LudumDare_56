using System.Runtime.CompilerServices;
using UnityEngine;
public interface IMovable
{
    public Vector2 Position { get; }
    public Vector2 Velocity { get; }
    public Vector2 NextVelocity { get; set; }
    public float Speed { get; }

    public float Radius { get; }
    public int Health { get; }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public abstract void ApplyNextVelocityAndMove(in float delta);
}