using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IMovable
{
    [field: SerializeField] public float Speed { get; protected set; } = 2f;
    public Vector2 Position { 
        get => new Vector2(transform.position.x, transform.position.z); 
        protected set => transform.position = new Vector3(value.x, 0, value.y); }
    public Vector2 Velocity { get;  protected set; }
    public Vector2 NextVelocity { get; set; }
    
    public float Radius => 0.5f;
    public int Health { get; protected set; } = 100;
    //public Vector2 InputDirection { get; internal set; }

    public void ApplyNextVelocityAndMove(in float delta)
    {
        Velocity = NextVelocity;
        Position += Velocity * delta * Speed;
    }

}
