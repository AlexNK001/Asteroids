using System;

public class ActionObserver<T> : IObserver<T>
{
    private readonly Action<T> _onNext;

    public ActionObserver(Action<T> onNext)
    {
        _onNext = onNext;
    }

    public void OnNext(T value)
    {
        _onNext?.Invoke(value);
    }

    public void OnError(Exception error)
    {
    }

    public void OnCompleted()
    {
    }
}