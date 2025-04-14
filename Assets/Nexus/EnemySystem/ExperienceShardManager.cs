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
        int numberOfBigShards = shardValue / 10;
        int numberOfSmallShards = shardValue % 10;

        // Spawn big shards (each worth 10)
        for (int i = 0; i < numberOfBigShards; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            );

            GameObject exp = ObjectPooler.Instance.SpawnFromPool(gameObject, position + randomOffset, Quaternion.identity);
            exp.GetComponent<ExperienceParticle>().experience = 10;
            exp.GetComponent<ExperienceParticle>().DetermineScaleAndColor();
        }

        // Spawn small shards (each worth 1)
        for (int i = 0; i < numberOfSmallShards; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            );

            GameObject exp = ObjectPooler.Instance.SpawnFromPool(gameObject, position + randomOffset, Quaternion.identity);
            exp.GetComponent<ExperienceParticle>().experience = 1;
            exp.GetComponent<ExperienceParticle>().DetermineScaleAndColor();
        }
    }

    private void UpdatePlayerXp(int value)
    {
        //TheHero.Instance.AddExperience(value);
        Debug.Log($"Player gained {value} experience");
    }
}