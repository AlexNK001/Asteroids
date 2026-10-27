public class PauseStateChanged
{
    public PauseStateChanged(bool isPaused)
    {
        IsPaused = isPaused;
    }

    public bool IsPaused { get;  }
}