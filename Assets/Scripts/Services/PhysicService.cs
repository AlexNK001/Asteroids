using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PhysicService : IDisposable, ITickable, IFixedTickable
{
    private readonly SignalBus _signalBus;
    private readonly FieldBoundaryConfig _config;
    private readonly ContactHandler _contactHandler;

    private readonly List<BaseObject> _elements = new();
    // private bool _isPausa;

    [Inject]
    public PhysicService(SignalBus signalBus, Config config, Ship ship, ContactHandler contactHandler)
    {
        _elements.Add(ship); //убрать
        _signalBus = signalBus;
        _config = config.FieldBoundary;
        _contactHandler = contactHandler;
        Subscribe();
    }

    public void Dispose()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _signalBus.Subscribe<ObjectCreatedSignal>(OnAdd);
        // _signalBus.Subscribe<FrameUpdateSignal>(OnUpdateElements);
        // _signalBus.Subscribe<FixedUpdateSignal>(OnFixedUpdate);
        _signalBus.Subscribe<ObjectReleaseSignal>(OnRemove);
    }

    private void Unsubscribe()
    {
        _signalBus.Unsubscribe<ObjectCreatedSignal>(OnAdd);
        // _signalBus.Unsubscribe<FrameUpdateSignal>(OnUpdateElements);
        // _signalBus.Unsubscribe<FixedUpdateSignal>(OnFixedUpdate);
        _signalBus.Unsubscribe<ObjectReleaseSignal>(OnRemove);
    }

    // private void OnUpdateElements(FrameUpdateSignal signal)
    // {
    //     
    //    
    //     foreach (var element in _elements)
    //     {
    //         element.Move(signal.DeltaTime); //
    //     }
    // }

    // private void OnFixedUpdate(FixedUpdateSignal signal)
    // {
    //     // if (_isPausa)
    //     // {
    //     //     return;
    //     // }
    //
    //     for (int i = 0; i < _elements.Count; i++)
    //     {
    //         ContainWithinBoundaries(_elements[i]);
    //     }
    //
    //     for (int i = 0; i < _elements.Count; i++)
    //     {
    //         BaseObject first = _elements[i];
    //
    //         for (int j = i + 1; j < _elements.Count; j++)
    //         {
    //             BaseObject second = _elements[j];
    //
    //             if (HaveContact(first, second))
    //             {
    //                 _contactHandler.Handle(first, second);
    //                 ResolveCollision(first, second);
    //             }
    //         }
    //     }
    // }

    private void OnAdd(ObjectCreatedSignal signal)
    {
        _elements.Add(signal.Element);
    }

    private void OnRemove(ObjectReleaseSignal signal)
    {
        _elements.Remove(signal.ReleasedObject);
    }

    private bool HaveContact(BaseObject first, BaseObject second)
    {
        float distance = Vector2.Distance(first.Position, second.Position);
        return distance < first.Radius + second.Radius;
    }

    private void ResolveCollision(BaseObject first, BaseObject second)
    {
        Vector2 normal = (first.Position - second.Position).normalized;

        Vector2 oldFirstDirection = first.LookDirection;
        Vector2 oldSecondDirection = second.LookDirection;

        Vector2 relativeDirection = oldFirstDirection - oldSecondDirection;
        float velAlongNormal = Vector2.Dot(relativeDirection, normal);

        Vector2 newFirstDirection = oldFirstDirection + velAlongNormal * normal;
        //first.Contact(second, newFirstDirection);

        Vector2 newSecondDirection = oldSecondDirection - velAlongNormal * normal;
        //second.Contact(first, newSecondDirection);
    }

    private void ContainWithinBoundaries(BaseObject cell)
    {
        Vector2 position = cell.Position;

        if (position.y > _config.Top)
        {
            position.y = _config.Bottom;
        }
        else if (position.y < _config.Bottom)
        {
            position.y = _config.Top;
        }
        else if (position.x > _config.Right)
        {
            position.x = _config.Left;
        }
        else if (position.x < _config.Left)
        {
            position.x = _config.Right;
        }
        else
        {
            return;
        }

        cell.Teleport(position);
    }

    public void Tick()
    {
       Debug.Log("Tick()");
       foreach (var element in _elements)
       {
           element.Move(Time.deltaTime); //signal.DeltaTime
       }
    }

    public void FixedTick()
    {
        Debug.Log("FixedTick()");
        for (int i = 0; i < _elements.Count; i++)
        {
            ContainWithinBoundaries(_elements[i]);
        }

        for (int i = 0; i < _elements.Count; i++)
        {
            BaseObject first = _elements[i];

            for (int j = i + 1; j < _elements.Count; j++)
            {
                BaseObject second = _elements[j];

                if (HaveContact(first, second))
                {
                    _contactHandler.Handle(first, second);
                    ResolveCollision(first, second);
                }
            }
        }
    }
}