using System.Reflection;

public struct PublisherInfo
{
    public readonly object Owner;
    public readonly FieldInfo Field;

    public PublisherInfo(object owner, FieldInfo field)
    {
        Field = field;
        Owner = owner;
    }

    public object GetInstance()
    {
        return Field.GetValue(Owner);
    }
}