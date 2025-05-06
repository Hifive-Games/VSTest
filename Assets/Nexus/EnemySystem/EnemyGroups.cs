using UnityEngine;

public enum SpawnBehavior
{
    Cluster,       // existing mode
    MaintainCount  // new mode: keep X enemies alive
}

[CreateAssetMenu(menuName = "Enemy System/Spawn Phase")]
public class SpawnPhaseData : ScriptableObject
{
    public SpawnBehavior behavior = SpawnBehavior.Cluster;

    public float startTime;
    public float duration;

    [Header("Cluster Mode")]
    public float spawnInterval;
    public int maxClusterGroups;

    [Header("Maintain-Count Mode")]
    public int targetEnemyCount;
    public float refillThreshold = 0.15f;
    public float maintainSpawnInterval = 1f;

    [Header("Enemy Groups")]
    [Tooltip("The enemy groups to spawn in this phase.")]
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