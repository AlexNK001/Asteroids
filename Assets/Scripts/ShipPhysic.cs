using UnityEngine;

public class ShipPhysic : PhysicCell
{
    public override void SetDeltaTime(float deltaTime)
    {
        Vector3 translation = _direction * (_speed * deltaTime);
        transform.Translate(translation, Space.World);
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
}