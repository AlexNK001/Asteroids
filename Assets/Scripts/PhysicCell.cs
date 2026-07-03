using System;
using UnityEngine;

public class PhysicCell : MonoBehaviour
{
    [SerializeField] protected float _radius;
    [SerializeField] protected Vector2 _direction;
    [SerializeField] protected float _speed;
    
    public event Action<PhysicCell> Destroyed;

    public float Radius => _radius;
    public Vector2 Direction => _direction;

    public virtual void SetDeltaTime(float deltaTime)
    {
        Vector2 translation = _direction * (_speed * deltaTime);
        transform.Translate(translation);
    }

    public virtual void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }

    public virtual void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public virtual void Contact(PhysicCell second, Vector2 direction)
    {
        SetDirection(direction);
    }

    public virtual void Leave(Vector2 teleportPosition)
    {
        transform.position = teleportPosition;
    }

    protected void RaiseDestroyed()
    {
        Destroyed.Invoke(this);
    }
}