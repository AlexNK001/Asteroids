using UnityEngine;

public class BigAsteroid : PhysicBody
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private  float _radius;
    public override float Radius => _radius;
    public override BodyType BodyType => BodyType.BigAsteroid;

    public override void Move(float deltaTime)
    {
        Vector3 translation = LookDirection * (deltaTime * _speed);
        transform.Translate(translation, Space.World);
    }
}