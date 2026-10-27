public class ShipSpeedChangedSignal
{
    public ShipSpeedChangedSignal(float speed,float normalizedSpeed)
    {
        Speed = speed;
        NormalizedSpeed = normalizedSpeed;
    }

    public float Speed { get; }
    public float NormalizedSpeed { get; }
}