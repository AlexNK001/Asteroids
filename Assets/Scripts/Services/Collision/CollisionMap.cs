using System;
using System.Collections.Generic;

public sealed class CollisionMap
{
    private readonly Dictionary<BodyType, int> _indexByKey;
    private readonly Action<PhysicBody, PhysicBody>[][] _array;

    public CollisionMap()
    {
        var keys = new[]
        {
            BodyType.Ship,
            BodyType.BigAsteroid,
            BodyType.SmallAsteroid,
            BodyType.Rocket,
            BodyType.Laser,
            BodyType.Ufo
        };

        _indexByKey = new Dictionary<BodyType, int>(keys.Length);

        for (int i = 0; i < keys.Length; i++)
        {
            _indexByKey[keys[i]] = i;
        }

        _array = new Action<PhysicBody, PhysicBody>[keys.Length][];

        for (int i = 0; i < keys.Length; i++)
        {
            _array[i] = new Action<PhysicBody, PhysicBody>[i + 1];
        }
    }

    public CollisionMap Registr(BodyType key1, BodyType key2, Action<PhysicBody, PhysicBody> action)
    {
        var result = GetPairKey(key1, key2);
        _array[result.row][result.column] = action;

        return this;
    }

    public void Invoke(PhysicBody first, PhysicBody second)
    {
        var result = GetPairKey(first.BodyType, second.BodyType);

        if (result.swapped)
        {
            (first, second) = (second, first);
        }
        
        _array[result.row][result.column].Invoke(first, second);
    }
    
    private (int row, int column, bool swapped) GetPairKey(BodyType key1, BodyType key2)
    {
        int index1 = _indexByKey[key1];
        int index2 = _indexByKey[key2];

        return index1 > index2 ? (index1, index2, true) : (index2, index1, false);
    }
}