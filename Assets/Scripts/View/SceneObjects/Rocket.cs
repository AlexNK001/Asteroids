using UnityEngine;

public class Rocket : BaseObject
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _radius = 3f;
    public override float Radius => _radius;
    public override ObjectType ObjectType => ObjectType.Rocket;

    public override void Move(float deltaTime)
    {
        Vector3 translation = LookDirection * (deltaTime * _speed);
        transform.Translate(translation, Space.World);
    }
}