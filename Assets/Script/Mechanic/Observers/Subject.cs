using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    protected List<IObserver> _observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        Debug.Log("Added Observer " + observer.ToString());
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }
 
    protected void NotifyEvents<T>(Events gameEvent, T parameter)
    {
        _observers.ForEach((_observers) =>
        {
            _observers.OnNotify(gameEvent, parameter);
        });
    }
    protected void NotifyEvents<T1, T2>(Events gameEvent, T1 param1, T2 param2)
    {
        _observers.ForEach(observer =>
        {
            observer.OnNotify(gameEvent, param1, param2);
        });
    }

    protected void NotifyEvents(Events gameEvent)
    {
        _observers.ForEach((_observers) =>
        {
            _observers.OnNotify(gameEvent);
        });
    }
}
