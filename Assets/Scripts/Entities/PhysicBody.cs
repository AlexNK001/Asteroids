using UnityEngine;

public abstract class PhysicBody : MonoBehaviour
{
    public abstract BodyType BodyType { get; }
    public abstract float Radius { get; }

    public Vector3 Position => transform.position;
    public Vector2 LookDirection => transform.up;

    public void PlaceAt(IPositionProvider provider)
    {
        transform.position = provider.Position;
        transform.rotation = provider.Rotation;
    }

    public void LookAt(Vector2 direction)
    {
        var rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = rotation;
    }

    public abstract void Move(float deltaTime);

    public void Wrap(Vector2 position)
    {
        transform.position = position;
    }
}