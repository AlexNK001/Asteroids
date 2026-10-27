using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class PublisherCollectionAttribute : BaseAttribute
{
    public PublisherCollectionAttribute(string key) : base(key)
    {
    }
}