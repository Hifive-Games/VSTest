
using UnityEngine;

public class GlobalGameEventManager : MonoBehaviour
{
    public static GlobalGameEventManager Instance { get; private set; }
    private GameEventSubject subject = new GameEventSubject();

    [SerializeField]
    private ExperienceShardManager shardManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            shardManager = new ExperienceShardManager();
            subject.Attach(shardManager);
        }
        else
        {
            Destroy(gameObject);
        }
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