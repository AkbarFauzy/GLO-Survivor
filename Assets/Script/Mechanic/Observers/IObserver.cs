using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    public void OnNotify(Events gameEvent);
    public void OnNotify<T>(Events gameEvent, T parameter);
}
