using System;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SubscriberCollectionAttribute : BaseAttribute
{
    public SubscriberCollectionAttribute(string key) : base(key)
    {
    }
    
}