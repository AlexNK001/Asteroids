using UnityEngine;

public class Ship : PhysicBody
{
    [SerializeField] private float _radius = 0.5f; 
    [SerializeField] private float _speed = 1.0f;
    private Vector2 _direction = Vector2.up;
    
    // public Vector3 Position => transform.position;
    // public Quaternion Rotation => transform.rotation;
    public override BodyType BodyType => BodyType.Ship;
    public override float Radius => _radius;
    public Vector2 LookDirection => transform.up;

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public override void Move(float deltaTime)//
    {
        Vector3 translation = _direction * (deltaTime * _speed);
        transform.Translate(translation, Space.World);
    }


    // public override void Teleport(Vector2 position)
    // {
    //     transform.position = position;
    // }

    public void LookAt(Vector2 direction)
    {
        _direction =  direction;
    }

    public void LookBack()
    {
        _direction *= -1f;
    }
}