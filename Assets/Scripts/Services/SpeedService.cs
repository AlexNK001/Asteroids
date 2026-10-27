using System;
using UnityEngine;
using Zenject;

public sealed class SpeedService : IDisposable,ITickable
{
    private readonly SignalBus _signalBus;
    private readonly SpeedConfig _config;

    private float _speed;
    private bool _isOppositeMovement;
    private bool _isMoveForwardKeyPressed;
    private bool _isMoveBackwardKeyPressed;

    public SpeedService(SignalBus signalBus, Config config)
    {
        _signalBus = signalBus;
        _config = config.Speed;
        //Subscribe();
    }

    public void Dispose()
    {
        //Unsubscribe();
    }

    // private void Subscribe()
    // {
    //     _signalBus.Subscribe<FrameUpdateSignal>(Update);
    // }
    //
    // private void Unsubscribe()
    // {
    //     _signalBus.Unsubscribe<FrameUpdateSignal>(Update);
    // }

    private void Update()
    {
        if (_isMoveForwardKeyPressed)
        {
            if (_isOppositeMovement)
            {
                _speed -= _config.SpeedDropOppositeMovement * Time.deltaTime;
            }
            else
            {
                _speed += _config.AccelerationForward * Time.deltaTime;
            }
        }
        else if (_isMoveBackwardKeyPressed)
        {
            _speed -= _config.DecelerationBackward * Time.deltaTime;
        }
        else
        {
            _speed -= _config.FrictionDrag * Time.deltaTime;
        }

        _speed = Mathf.Clamp(_speed, _config.MinSpeed, _config.MaxSpeed);
        float normalizedSpeed = Mathf.InverseLerp(_config.MinSpeed, _config.MaxSpeed, _speed);//
        _signalBus.Fire(new ShipSpeedChangedSignal(_speed, normalizedSpeed));
    }

    public void Tick()
    {
        Update();
    }
}