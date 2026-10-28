public class BodyCreatedSignal
{
    public BodyCreatedSignal(PhysicBody element)
    {
        Element = element;
    }

    public PhysicBody Element { get; }
}