using System;
using Zenject;

public class PauseServiсe : IDisposable
{
    private readonly SignalBus _signalBus;
    private bool _isPaused;

    public PauseServiсe(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<PauseButtonPressedSignal>(OnTogglePauseState);
    }

    private void OnTogglePauseState()
    {
        _isPaused = !_isPaused;
        _signalBus.Fire(new PauseStateChanged(_isPaused));
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<PauseButtonPressedSignal>(OnTogglePauseState);
    }
}