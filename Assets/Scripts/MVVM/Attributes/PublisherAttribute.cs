using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class PublisherAttribute : BaseAttribute
{
    public PublisherAttribute(string key) : base(key)
    {
    }
}