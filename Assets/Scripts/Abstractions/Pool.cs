using UnityEngine;
using UnityEngine.Pool;

public sealed class Pool<T> where T : BaseObject
{
    private readonly T _prefab;
    private readonly ObjectPool<T> _objectPool;

    public Pool(T prefab)
    {
        _prefab = prefab;
        _objectPool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public T Get()
    {
        var objectPool = _objectPool.Get();
        return objectPool;
    }

    public void Release(T physic)
    {
        _objectPool.Release(physic);
    }

    private void OnDestroy(T physic)
    {
        Object.Destroy(physic.gameObject);
    }

    private void OnRelease(T physic)
    {
        physic.gameObject.SetActive(false);
    }

    private void OnGet(T physic)
    {
        physic.gameObject.SetActive(true);
    }

    private T OnCreate()
    {
        return Object.Instantiate(_prefab);
    }
}