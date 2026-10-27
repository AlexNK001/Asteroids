public class ObjectReleaseSignal
{
    public ObjectReleaseSignal(BaseObject releasedObject)
    {
        ReleasedObject = releasedObject;
    }

    public BaseObject ReleasedObject { get; }
}