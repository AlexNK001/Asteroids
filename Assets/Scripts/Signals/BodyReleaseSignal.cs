public class BodyReleaseSignal
{
    public BodyReleaseSignal(PhysicBody releasedBody)
    {
        ReleasedBody = releasedBody;
    }

    public PhysicBody ReleasedBody { get; }
}