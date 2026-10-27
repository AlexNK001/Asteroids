using System;
using System.Collections.Generic;
using Zenject;

public sealed class ScoreService : IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly PriceConfig _priceConfig;
    private readonly Dictionary<ObjectType, int> _rewards;

    public ScoreService(SignalBus signalBus, Config config)
    {
        _signalBus = signalBus;
        _priceConfig = config.Price;
        _signalBus.Subscribe<EnemyDefeatedSignal>(OnAdd);

        _rewards = new Dictionary<ObjectType, int>
        {
            { ObjectType.SmallAsteroid, _priceConfig.SmallAsteroid },
            { ObjectType.BigAsteroid, _priceConfig.BigAsteroid },
            { ObjectType.Ufo, _priceConfig.Ufo }
        };
    }

    private void OnAdd(EnemyDefeatedSignal defeatedSignal)
    {
        int score = _rewards[defeatedSignal.Enemy.ObjectType];
        var signal = new ScoreChangedSignal(score);
        _signalBus.Fire(signal);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<EnemyDefeatedSignal>(OnAdd);
    }
}