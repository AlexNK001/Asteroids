using UnityEngine;

public  class SpawnPoint : MonoBehaviour, IPositionProvider
{
    [SerializeField] private ObjectType _spawnObjet;
    
    public ObjectType SpawnObjet => _spawnObjet;
    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
}