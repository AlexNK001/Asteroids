using System;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SubscriberAttribute : BaseAttribute
{
    public SubscriberAttribute(string key) : base(key)
    {
    }
}