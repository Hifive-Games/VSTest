
using UnityEngine;
public interface IGameEventObserver
{
    void OnNotify(string eventType);
    void OnNotify(string eventType, int value);
    void OnNotify(string eventType, int value, Vector3 position, GameObject gameObject);
}