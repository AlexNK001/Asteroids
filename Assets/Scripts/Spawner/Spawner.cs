using UnityEngine;
using Zenject;

public sealed class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnPoint _rocketSpawnPoint;
    [SerializeField] private SpawnPoint[] _points;

    private Pool<SmallAsteroid> _smallAsteroidPool;
    private Pool<BigAsteroid> _bigAsteroidPool;
    private Pool<Ufo> _ufoPool;
    private Pool<Rocket> _rocketPool;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus, PoolContainer pools)
    {
        _signalBus = signalBus;
        _smallAsteroidPool = pools.SmallAsteroidPool;
        _bigAsteroidPool = pools.BigAsteroidPool;
        _ufoPool = pools.UfoPool;
        _rocketPool = pools.RocketPool;

        Subscribe();
    }

    private void Start()
    {
        SpawnInitialObjects();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void SpawnInitialObjects()
    {
        if (_points == null) return;

        foreach (var point in _points)
        {
            if (point == null) continue;

            switch (point.SpawnObjet)
            {
                case ObjectType.Rocket:
                    Create(point, _rocketPool);
                    break;
                
                case ObjectType.Laser:
                    break;
                
                case ObjectType.SmallAsteroid:
                    break;
                
                case ObjectType.BigAsteroid:
                    Create(point, _bigAsteroidPool);
                    break;
                
                case ObjectType.Ufo:
                    Create(point, _ufoPool);
                    break;
                
                default:
                    Debug.LogWarning($"Неизвестный тип точки спавна: {point.GetType().Name}", point);
                    break;
            }
        }
    }

    private void OnRelease(ObjectReleaseSignal signal)
    {
        switch (signal.ReleasedObject)
        {
            case Rocket rocket:
                _rocketPool.Release(rocket);
                break;
            
            case SmallAsteroid smallAsteroid:
                _smallAsteroidPool.Release(smallAsteroid);
                break;
            
            case BigAsteroid bigAsteroid:
                _bigAsteroidPool.Release(bigAsteroid);
                break;
            
            case Ufo ufo:
                _ufoPool.Release(ufo);
                break;
            
            default:
                Debug.LogError($"Нет пула для объекта типа {signal.ReleasedObject?.GetType().Name}");
                break;
        }
    }

    private void OnSpawnRocket(RocketFiredSignal signal)
    {
        if (_rocketSpawnPoint == null)
        {
            Debug.LogError("RocketSpawnPoint не назначен", this);
            return;
        }

        Create(_rocketSpawnPoint, _rocketPool);
    }

    private void Create<T>(SpawnPoint point, Pool<T> pool) where T : BaseObject
    {
        var spawnedObject = pool.Get();
        spawnedObject.PlaceAt(point);
        _signalBus.Fire(new ObjectCreatedSignal(spawnedObject));
    }

    private void Subscribe()
    {
        _signalBus.Subscribe<RocketFiredSignal>(OnSpawnRocket);
        _signalBus.Subscribe<ObjectReleaseSignal>(OnRelease);
    }

    private void Unsubscribe()
    {
        _signalBus.Unsubscribe<RocketFiredSignal>(OnSpawnRocket);
        _signalBus.Unsubscribe<ObjectReleaseSignal>(OnRelease);
    }
}