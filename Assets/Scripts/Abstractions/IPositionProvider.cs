using UnityEngine;

public interface IPositionProvider
{
    Vector3 Position { get; }
    Quaternion Rotation { get; }
}