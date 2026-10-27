using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class HealthService : IDisposable
{
    private int _currentHealth;
    private int _maxHealth;
    private bool _isImmortalized;

    private readonly SignalBus _signalBus;

    public HealthService(SignalBus signalBus)
    {
        _currentHealth = 3;
        _maxHealth = 3;

        _signalBus = signalBus;
        _signalBus.Subscribe<ShipTakeDamagedSignal>(OnTakeDamage);
    }

    private async UniTaskVoid Immortalize()
    {
        _isImmortalized = true;
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        _isImmortalized = false;
    }

    private void OnTakeDamage()
    {
        if (_isImmortalized == false)
        {
            _currentHealth--;
            Immortalize().Forget();
        }

        if (_currentHealth <= 0)
        {
            Debug.Log("death");
        }
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<ShipTakeDamagedSignal>(OnTakeDamage);
    }
}