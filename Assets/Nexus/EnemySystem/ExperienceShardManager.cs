using UnityEngine;
public class ExperienceShardManager : IGameEventObserver
{
    public void OnNotify(string eventType)
    {
    }

    public void OnNotify(string eventType, int value)
    {
        if (eventType == "PlayerGetExperiance")
            UpdatePlayerXp(value);
    }

    public void OnNotify(string eventType, int value, Vector3 position, GameObject gameObject)
    {
        if (eventType == "EnemyDied")
        {
            SpawnShards(value, position, gameObject);
        }
    }

    private void SpawnShards(int shardValue, Vector3 position, GameObject gameObject)
    {
        for (int i = 0; i < shardValue; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            );
            ObjectPooler.Instance.SpawnFromPool(gameObject, position + randomOffset, Quaternion.identity);
        }
    }

    private void UpdatePlayerXp(int value)
    {
        Player.Instance.AddExperience(value);
    }
}