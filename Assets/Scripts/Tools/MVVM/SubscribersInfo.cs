using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SubscribersInfo : IEnumerable<SubscriberInfo>
{
    private readonly List<SubscriberInfo> _list = new();

    public void Add(SubscriberInfo item)
    {
        _list.Add(item);
    }

    public void Add(object instance, MethodInfo method)
    {
        SubscriberInfo item = new(instance, method);
        _list.Add(item);
    }

    public IEnumerator<SubscriberInfo> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}