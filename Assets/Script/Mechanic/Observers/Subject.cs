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

    protected void NotifyEvents(Events gameEvent)
    {
        _observers.ForEach((_observers) =>
        {
            _observers.OnNotify(gameEvent);
        });
    }
}
