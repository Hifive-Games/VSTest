using UnityEngine;

[CreateAssetMenu(menuName = "Enemy System/Spawn Phase")]
public class SpawnPhaseData : ScriptableObject
{
    [Header("Timing")]
    public float startTime;
    public float duration;

    [Header("Spawning")]
    public float spawnInterval;
    public int maxClusterGroups;
    public EnemySpawnGroup[] enemyGroups;
}

[System.Serializable]
public struct EnemySpawnGroup
{
    public EnemyDataSO enemyData;
    public int amount;
    public float minSpwanRadius;
    public float maxSpawnRadius;
}