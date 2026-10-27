using UnityEngine;

public abstract class BaseObject : MonoBehaviour
{
    // [SerializeField] protected float _speed ;
    public abstract ObjectType ObjectType { get; }
    public abstract float Radius { get; }
    
    public virtual Vector3 Position => transform.position;
    public virtual Quaternion Rotation => transform.rotation;
    public virtual Vector2 LookDirection => transform.up;
    
    public void PlaceAt(IPositionProvider provider)
    {
        transform.position = provider.Position;
        transform.rotation = provider.Rotation;
    }

    public abstract void Move(float deltaTime);//
    // {
    //     Vector3 translation = LookDirection * (deltaTime * _speed);
    //     transform.Translate(translation, Space.World);
    // }

    public void Teleport(Vector2 position)
    {
        transform.position = position;
    }
}

