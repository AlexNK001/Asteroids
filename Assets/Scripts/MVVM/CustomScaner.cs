using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniRx;
using UnityEngine;

public sealed class CustomScanner
{
    private readonly List<IDisposable> _handlers = new();

    private readonly BindingFlags _bindingAttribute =
        BindingFlags.Public |
        BindingFlags.NonPublic |
        BindingFlags.DeclaredOnly |
        BindingFlags.Instance;

    private readonly Dictionary<string, PublisherInfo> _publishers = new();
    private readonly Dictionary<string, SubscribersInfo> _subscribers = new();
    private readonly Dictionary<string, PublisherInfo> _publishersCollection = new();
    private readonly Dictionary<string, SubscribersInfo> _subscribersCollection = new();

    public void Contact(object[] objects)
    {
        foreach (var item in objects)
        {
            Collect(item);
        }
        // Debug.Log($"_publishers{_publishers.Count} _subscribers{_subscribers.Count}");
        Connect();
        // ConnectCollection();
    }

    private void Collect(object instance)
    {
        // _publishers = new();
        // _subscribers = new();
        // _publishersCollection = new();
        // _subscribersCollection = new();

        MemberInfo[] members = instance.GetType().GetMembers(_bindingAttribute);

        foreach (MemberInfo member in members)
        {
            var attribute = member.GetCustomAttribute<BaseAttribute>();

            switch (attribute)
            {
                case PublisherAttribute publisherAttribute when member is FieldInfo fieldInfo:
                    _publishers.Add(publisherAttribute.Key, new(instance, fieldInfo));
                    break;

                case SubscriberAttribute subscriberAttribute when member is MethodInfo methodInfo:
                    if (_subscribers.ContainsKey(subscriberAttribute.Key) == false)
                        _subscribers.Add(subscriberAttribute.Key, new SubscribersInfo());

                    _subscribers[subscriberAttribute.Key].Add(new SubscriberInfo(instance, methodInfo));
                    break;

                case PublisherCollectionAttribute collectionAttribute when member is FieldInfo fieldInfo:
                    _publishersCollection.Add(collectionAttribute.Key, new(instance, fieldInfo));
                    break;

                case SubscriberCollectionAttribute collectionAttribute when member is MethodInfo methodInfo:
                    if (_subscribersCollection.ContainsKey(collectionAttribute.Key) == false)
                        _subscribersCollection.Add(collectionAttribute.Key, new SubscribersInfo());

                    _subscribersCollection[collectionAttribute.Key].Add(instance, methodInfo);
                    break;
            }
        }

        // Connect();
        // ConnectCollection();
    }

    private void Connect()
    {
        foreach (var publisher in _publishers)
        {
            object reactiveField = publisher.Value.GetInstance();
            Type genericTypeParam = reactiveField.GetType().GetGenericArguments()[0];
            Type delegateType = typeof(Action<>).MakeGenericType(genericTypeParam);
            Type observerType = typeof(ActionObserver<>).MakeGenericType(genericTypeParam);
            Type observableType = typeof(IObservable<>).MakeGenericType(genericTypeParam);
            Type[] types = { typeof(IObserver<>).MakeGenericType(genericTypeParam) };
            MethodInfo subscribeMethod = observableType.GetMethod("Subscribe", types);

            foreach (var subscriber in _subscribers[publisher.Key])
            {
                var handler = Delegate.CreateDelegate(delegateType, subscriber.Instance, subscriber.Method);
                object observer = Activator.CreateInstance(observerType, handler);
                IDisposable subscription = (IDisposable)subscribeMethod.Invoke(reactiveField, new[] { observer });
                _handlers.Add(subscription);
            }
        }
    }

    private void ConnectCollection()
    {
        foreach (var publisher in _publishersCollection)
        {
            string key = publisher.Key;

            object reactiveCollection = publisher.Value.GetInstance();
            Type elementType = reactiveCollection.GetType().GetGenericArguments()[0];

            MethodInfo observeAddMethod = reactiveCollection.GetType().GetMethod("ObserveAdd");
            
            Type observableType = typeof(IObservable<>)
                .MakeGenericType(typeof(CollectionAddEvent<>)
                    .MakeGenericType(elementType));
            
            Type funcType = typeof(Func<>).MakeGenericType(observableType);
            Delegate observeAddDelegate = Delegate.CreateDelegate(funcType, reactiveCollection, observeAddMethod);

            object observable = observeAddDelegate.DynamicInvoke();

            foreach (var subscriber in _subscribersCollection[key])
            {
                Type eventType = typeof(CollectionAddEvent<>).MakeGenericType(elementType);
                Type actionType = typeof(Action<>).MakeGenericType(eventType);

                Delegate handler = Delegate.CreateDelegate(actionType, subscriber.Instance, subscriber.Method);

                MethodInfo subscribeMethod = typeof(ObservableExtensions)
                    .GetMethods()
                    .First(m => m.Name == "Subscribe" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(eventType);

                IDisposable subscription = (IDisposable)subscribeMethod.Invoke(null, new[] { observable, handler });
                _handlers.Add(subscription);
            }
        }
    }
}