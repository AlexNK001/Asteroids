using System;

public abstract class BaseAttribute : Attribute
{
    protected BaseAttribute(string key)
    {
        Key = key;
    }

    public string Key { get; }
}