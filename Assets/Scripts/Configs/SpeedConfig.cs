public sealed class SpeedConfig
{
    public SpeedConfig(float accelerationForward, float decelerationBackward, float speedDropOppositeMovement,
        float frictionDrag, float maxSpeed, float minSpeed)
    {
        AccelerationForward = accelerationForward;
        DecelerationBackward = decelerationBackward;
        SpeedDropOppositeMovement = speedDropOppositeMovement;
        FrictionDrag = frictionDrag;
        MaxSpeed = maxSpeed;
        MinSpeed = minSpeed;
    }

    public float AccelerationForward { get; }
    public float DecelerationBackward { get; }
    public float SpeedDropOppositeMovement { get; }
    public float FrictionDrag { get; }
    public float MaxSpeed { get; }
    public float MinSpeed { get; }
}