using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class HealthService : IDisposable
{
    private int _currentHealth;
    private int _maxHealth;
    private bool _isInvulnerability;

    private readonly SignalBus _signalBus;

    public HealthService(SignalBus signalBus)
    {
        _currentHealth = 3;
        _maxHealth = 3;

        _signalBus = signalBus;
        _signalBus.Subscribe<ShipTakeDamagedSignal>(OnTakeDamage);
    }

    private async UniTaskVoid GrantInvulnerability()
    {
        _isInvulnerability = true;
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        _isInvulnerability = false;
    }

    private void OnTakeDamage()
    {
        if (_isInvulnerability == false)
        {
            _currentHealth--;
            GrantInvulnerability().Forget();
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