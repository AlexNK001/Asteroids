using UnityEngine;

public class Ufo : BaseObject
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _radius = 0.5f;

    public override ObjectType ObjectType => ObjectType.Ufo;
    public override float Radius => _radius;
    
    public override void Move(float deltaTime)
    {
        Vector3 translation = LookDirection * (deltaTime * _speed);
        transform.Translate(translation, Space.World);
    }
}