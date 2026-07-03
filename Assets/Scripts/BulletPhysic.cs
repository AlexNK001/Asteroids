using UnityEngine;

public class BulletPhysic : PhysicCell
{
    public override void SetDeltaTime(float deltaTime)
    {
        transform.position += transform.up * (deltaTime * _speed);
    }

    public override void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void Contact(PhysicCell second, Vector2 direction)
    {
        switch (second)
        {
            case ShipPhysic ship:
                break;

            case BulletPhysic bullet:
                break;

            default:
                SetDirection(direction);
                break;
        }
    }

    public override void Leave(Vector2 teleportPosition)
    {
        Debug.Log("Leaving map");
        RaiseDestroyed();
        Destroy(gameObject);//
    }
}