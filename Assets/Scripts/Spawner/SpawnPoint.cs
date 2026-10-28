using UnityEngine;

public  class SpawnPoint : MonoBehaviour, IPositionProvider
{
    [SerializeField] private BodyType _spawnObjet;
    
    public BodyType SpawnObjet => _spawnObjet;
    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
}