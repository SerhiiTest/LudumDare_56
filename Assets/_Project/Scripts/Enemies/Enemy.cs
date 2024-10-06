using System;
using UnityEngine;
public class Enemy : MonoBehaviour, IMovable
{
    public EnemyType Type { get; protected set; }
    //public Vector2 Position { get; protected set; }
    public Vector2 Position { get => new Vector2(transform.position.x, transform.position.z); protected set => transform.position = new Vector3(value.x, 0, value.y); }
    public Vector2 Velocity { get; protected set; }
    public Vector2 NextVelocity { get; set; }
    [field: SerializeField] public float Speed { get; protected set; } = 1;
    public float Radius { get; protected set; } = 0.25f;
    public int Health { get; protected set; } = 5;
    
    public void ApplyNextVelocityAndMove(in float delta)
    {
        Velocity = NextVelocity;
        Position += Velocity * delta * Speed;
    }

    public EnemyType EnemyType { get; protected set; }

}