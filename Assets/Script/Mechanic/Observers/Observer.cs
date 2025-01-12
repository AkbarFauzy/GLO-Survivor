using System;
using System.Collections.Generic;

public class Observer : Subject, IObserver
{
    private Dictionary<Events, Delegate> _eventHandlers = new Dictionary<Events, Delegate>();

    public void Subscribe<T>(Events gameEvent, Action<T> handler)
    {
        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            _eventHandlers[gameEvent] = existingDelegate as Action<T> + handler;
        }
        else
        {
            _eventHandlers[gameEvent] = handler;
        }
    }

    public void Subscribe(Events gameEvent, Action handler)
    {
        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            _eventHandlers[gameEvent] = existingDelegate as Action + handler;
        }
        else
        {
            _eventHandlers[gameEvent] = handler;
        }
    }

    public void Unsubscribe<T>(Events gameEvent, Action<T> handler)
    {
        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            _eventHandlers[gameEvent] = existingDelegate as Action<T> - handler;

            // Remove the event if no handlers remain
            if (_eventHandlers[gameEvent] == null)
                _eventHandlers.Remove(gameEvent);
        }
    }

    public void OnNotify<T>(Events gameEvent, T parameter)
    {
        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            var handler = existingDelegate as Action<T>;
            handler?.Invoke(parameter);
        }
    }

    public void OnNotify(Events gameEvent)
    {
        if (_eventHandlers.TryGetValue(gameEvent, out var existingDelegate))
        {
            var handler = existingDelegate as Action;
            handler?.Invoke();
        }
    }
}
