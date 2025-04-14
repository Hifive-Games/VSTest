
using UnityEngine;

public class GlobalGameEventManager : MonoBehaviourSingleton<GlobalGameEventManager>
{
    private GameEventSubject subject = new GameEventSubject();

    [SerializeField]
    private ExperienceShardManager shardManager;

    private void Awake()
    {
        shardManager = new ExperienceShardManager();
        subject.Attach(shardManager);
    }

    public void Notify(string eventType, int value, Vector3 position, GameObject gameObject)
    {
        subject.Notify(eventType, value, position, gameObject);
    }

    public void Notify(string eventType, int value)
    {
        subject.Notify(eventType, value);
    }
}