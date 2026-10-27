using UnityEngine;

public class SmallAsteroid : BaseObject
{
    [SerializeField] private float _radius = 0.5f;
    public override void Move(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override ObjectType ObjectType => ObjectType.SmallAsteroid;
    public override float Radius => _radius;
}
