using System.Collections.Generic;
using UnityEngine;
public class GameEventSubject
{
    private List<IGameEventObserver> observers = new List<IGameEventObserver>();

    public void Attach(IGameEventObserver observer)
        => observers.Add(observer);

    public void Detach(IGameEventObserver observer)
        => observers.Remove(observer);

    public void Notify(string eventType)
    {
        foreach (var observer in observers)
            observer.OnNotify(eventType);
    }

    public void Notify(string eventType, int value)
    {
        foreach (var observer in observers)
            observer.OnNotify(eventType, value);
    }

    public void Notify(string eventType, int value, Vector3 position, GameObject gameObject)
    {
        foreach (var observer in observers)
            observer.OnNotify(eventType, value, position, gameObject);
    }
}