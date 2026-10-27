using System;
using System.Collections.Generic;

public sealed class TwinKeyList
{
    private readonly Dictionary<ObjectType, int> _indexByKey;
    private readonly Action<BaseObject, BaseObject>[][] _array;

    public TwinKeyList()
    {
        var keys = new[]
        {
            ObjectType.Ship,
            ObjectType.BigAsteroid,
            ObjectType.SmallAsteroid,
            ObjectType.Rocket,
            ObjectType.Laser,
            ObjectType.Ufo
        };

        _indexByKey = new Dictionary<ObjectType, int>(keys.Length);

        for (int i = 0; i < keys.Length; i++)
        {
            _indexByKey[keys[i]] = i;
        }

        _array = new Action<BaseObject, BaseObject>[keys.Length][];

        for (int i = 0; i < keys.Length; i++)
        {
            _array[i] = new Action<BaseObject, BaseObject>[i + 1];
        }
    }

    public TwinKeyList Registration(ObjectType key1, ObjectType key2, Action<BaseObject, BaseObject> action)
    {
        var result = GetOrderedIndices(key1, key2);
        _array[result.a][result.b] = action;

        return this;
    }

    public void InvokeSignal(BaseObject first, BaseObject second)
    {
        var result = GetOrderedIndices(first.ObjectType, second.ObjectType);

        if (result.swapped)
        {
            (first, second) = (second, first);
        }
        
        _array[result.a][result.b].Invoke(first, second);
    }
    
    private (int a, int b, bool swapped) GetOrderedIndices(ObjectType key1, ObjectType key2)
    {
        int index1 = _indexByKey[key1];
        int index2 = _indexByKey[key2];

        return index1 > index2 ? (index1, index2, true) : (index2, index1, false);
    }
}