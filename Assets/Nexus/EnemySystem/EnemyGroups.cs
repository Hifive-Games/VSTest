[System.Serializable]
public struct EnemySpawnGroup
{
    public EnemyDataSO enemyData;
    public int amount;
    public float minSpwanRadius;
    public float maxSpawnRadius;
}

[System.Serializable]
public struct SpawnPhase
{
    public float startTime;
    public float endTime;
    public float spawnInterval;
    public int maxClusterGroupLimit;
    public EnemySpawnGroup[] enemyGroups;
}