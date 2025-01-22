using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    public void Subscribe(Events gameEvent, Action handler);
    public void Subscribe<T>(Events gameEvent, Action<T> handler);
    public void Subscribe<T1, T2>(Events gameEvent, Action<T1, T2> handler);
    public void Unsubscribe<T>(Events gameEvent, Action<T> handler);
    public void Unsubscribe<T1, T2>(Events gameEvent, Action<T1, T2> handler);
    public void OnNotify(Events gameEvent);
    public void OnNotify<T>(Events gameEvent, T parameter);
    public void OnNotify<T1, T2>(Events gameEvent, T1 param1, T2 param2);
}
