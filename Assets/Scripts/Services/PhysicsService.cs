using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PhysicsService : IDisposable, ITickable, IFixedTickable
{
    private readonly SignalBus _signalBus;
    private readonly FieldBoundaryConfig _config;
    private readonly CollisionService _collisionService;

    private readonly List<PhysicBody> _bodies = new();
    private readonly List<(PhysicBody, PhysicBody)> _overlapBodies = new();

    [Inject]
    public PhysicsService(SignalBus signalBus, Config config, Ship ship, CollisionService collisionService)
    {
        _bodies.Add(ship); //убрать
        _signalBus = signalBus;
        _config = config.FieldBoundary;
        _collisionService = collisionService;
        Subscribe();
    }

    public void Dispose()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _signalBus.Subscribe<BodyCreatedSignal>(OnAdd);
        _signalBus.Subscribe<BodyReleaseSignal>(OnRemove);
    }

    private void Unsubscribe()
    {
        _signalBus.Unsubscribe<BodyCreatedSignal>(OnAdd);
        _signalBus.Unsubscribe<BodyReleaseSignal>(OnRemove);
    }

    private void OnAdd(BodyCreatedSignal signal)
    {
        _bodies.Add(signal.Element);
    }

    private void OnRemove(BodyReleaseSignal signal)
    {
        var body = signal.ReleasedBody;
        _bodies.Remove(body);

        for (int i = _overlapBodies.Count - 1; i >= 0; i--)
        {
            var bodyPair = _overlapBodies[i];

            if (ReferenceEquals(body, bodyPair.Item1) || ReferenceEquals(body, bodyPair.Item2))
            {
                _overlapBodies.RemoveAt(i);
            }
        }
    }

    private bool HasOverlap(PhysicBody first, PhysicBody second)
    {
        float distance = Vector2.Distance(first.Position, second.Position);
        return distance < first.Radius + second.Radius;
    }

    private void WrapWithinBoundaries(PhysicBody cell)
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

        cell.Wrap(position);
    }

    public void Tick()
    {
        foreach (var element in _bodies)
        {
            element.Move(Time.deltaTime);
        }
    }

    public void FixedTick()
    {
        for (int i = 0; i < _bodies.Count; i++)
        {
            WrapWithinBoundaries(_bodies[i]);
        }

        for (int i = 0; i < _bodies.Count; i++)
        {
            PhysicBody first = _bodies[i];

            for (int j = i + 1; j < _bodies.Count; j++)
            {
                PhysicBody second = _bodies[j];

                if (_overlapBodies.Contains((first, second)) == false && HasOverlap(first, second))
                {
                    _collisionService.Handle(first, second);
                    _overlapBodies.Add((first, second));
                }
            }
        }

        for (int i = _overlapBodies.Count - 1; i >= 0; i--)
        {
            var bodyPair = _overlapBodies[i];

            if (HasOverlap(bodyPair.Item1, bodyPair.Item2) == false)
            {
                _overlapBodies.RemoveAt(i);
            }
        }
    }
}