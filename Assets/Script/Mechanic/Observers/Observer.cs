using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class Observer : Subject, IObserver
{
    private ConcurrentDictionary<Events, Delegate> _eventHandlers = new ConcurrentDictionary<Events, Delegate>();

    public void Subscribe<T>(Events gameEvent, Action<T> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        _eventHandlers.AddOrUpdate(gameEvent, handler, (key, existingDelegate) => (existingDelegate as Action<T>) + handler);
    }

    // Subscribe for two parameters
    public void Subscribe<T1, T2>(Events gameEvent, Action<T1, T2> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        _eventHandlers.AddOrUpdate(gameEvent, handler, (key, existingDelegate) => Delegate.Combine(existingDelegate, handler));
    }

    public void Subscribe(Events gameEvent, Action handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        _eventHandlers.AddOrUpdate(gameEvent, handler, (key, existingDelegate) => (existingDelegate as Action) + handler);
    }

    public void Unsubscribe<T>(Events gameEvent, Action<T> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            var newDelegate = (existingDelegate as Action<T>) - handler;
            if (newDelegate == null)
            {
                _eventHandlers.TryRemove(gameEvent, out _);
            }
            else
            {
                _eventHandlers[gameEvent] = newDelegate;
            }
        }
    }

    // Unsubscribe for two parameters
    public void Unsubscribe<T1, T2>(Events gameEvent, Action<T1, T2> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, handler);
            if (newDelegate == null)
            {
                _eventHandlers.TryRemove(gameEvent, out _);
            }
            else
            {
                _eventHandlers[gameEvent] = newDelegate;
            }
        }
    }

    public void Unsubscribe(Events gameEvent, Action handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            var newDelegate = (existingDelegate as Action) - handler;
            if (newDelegate == null)
            {
                _eventHandlers.TryRemove(gameEvent, out _);
            }
            else
            {
                _eventHandlers[gameEvent] = newDelegate;
            }
        }
    }

    public void OnNotify<T>(Events gameEvent, T parameter)
    {
        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            (existingDelegate as Action<T>)?.Invoke(parameter);
        }
    }

    // Notify with two parameters
    public void OnNotify<T1, T2>(Events gameEvent, T1 param1, T2 param2)
    {
        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            (existingDelegate as Action<T1, T2>)?.Invoke(param1, param2);
        }
    }

    public void OnNotify(Events gameEvent)
    {
        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            (existingDelegate as Action)?.Invoke();
        }
    }
}
