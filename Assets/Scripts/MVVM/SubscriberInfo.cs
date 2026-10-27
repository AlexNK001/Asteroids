using System.Reflection;

public struct SubscriberInfo
{
    public readonly object Instance;
    public readonly MethodInfo Method;

    public SubscriberInfo(object instance, MethodInfo method)
    {
        Instance = instance;
        Method = method;
    }
}