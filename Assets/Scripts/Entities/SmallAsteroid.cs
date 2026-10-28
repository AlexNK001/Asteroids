using UnityEngine;

public class SmallAsteroid : PhysicBody
{
    [SerializeField] private float _radius = 0.5f;
    public override void Move(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override BodyType BodyType => BodyType.SmallAsteroid;
    public override float Radius => _radius;
}
