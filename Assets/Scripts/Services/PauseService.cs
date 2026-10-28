using System;
using Zenject;

public class PauseService : IDisposable
{
    private readonly SignalBus _signalBus;
    private bool _isPaused;

    public PauseService(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<PauseStateChangedSignal>(OnTogglePauseState);
    }

    private void OnTogglePauseState()
    {
        _isPaused = !_isPaused;
        _signalBus.Fire(new PauseStateChanged(_isPaused));
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<PauseStateChangedSignal>(OnTogglePauseState);
    }
}