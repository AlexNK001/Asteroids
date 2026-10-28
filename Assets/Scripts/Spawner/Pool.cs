using UnityEngine;
using UnityEngine.Pool;

public sealed class Pool<T> where T : PhysicBody
{
    private readonly T _prefab;
    private readonly ObjectPool<T> _bodyPool;

    public Pool(T prefab)
    {
        _prefab = prefab;
        _bodyPool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public T Get()
    {
        var objectPool = _bodyPool.Get();
        return objectPool;
    }

    public void Release(T physic)
    {
        _bodyPool.Release(physic);
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