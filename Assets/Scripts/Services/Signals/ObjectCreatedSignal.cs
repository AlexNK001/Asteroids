public class ObjectCreatedSignal
{
    public ObjectCreatedSignal(BaseObject element)
    {
        Element = element;
    }

    public BaseObject Element { get; }
}