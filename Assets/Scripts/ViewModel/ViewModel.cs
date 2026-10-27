using System;
using UniRx;
using Zenject;

public sealed class ViewModel : IDisposable
{
    private readonly SignalBus _signalBus;

    [Publisher(Keys.Score)] private readonly ReactiveProperty<string> _score;
    [Publisher(Keys.Speed)] private readonly ReactiveProperty<string> _speed;
    [Publisher(Keys.NormalizedSpeed)]  private readonly ReactiveProperty<float> _normalizedSpeed;

    public ViewModel(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _score = new ReactiveProperty<string>();
        _speed = new ReactiveProperty<string>();
        _normalizedSpeed = new ReactiveProperty<float>();
        
        _signalBus.Subscribe<ScoreChangedSignal>(OnHandleScoreChange);
        _signalBus.Subscribe<ShipSpeedChangedSignal>(OnHandleShipSpeedChange);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<ScoreChangedSignal>(OnHandleScoreChange);
        _signalBus.Unsubscribe<ShipSpeedChangedSignal>(OnHandleShipSpeedChange);
    }

    private void OnHandleScoreChange(ScoreChangedSignal signal)
    {
        _score.Value = signal.Score.ToString();
    }

    private void OnHandleShipSpeedChange(ShipSpeedChangedSignal signal)
    {
        _speed.Value = signal.Speed.ToString();
        _normalizedSpeed.Value = signal.NormalizedSpeed;
    }
}