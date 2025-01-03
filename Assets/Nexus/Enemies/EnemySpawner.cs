using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float minSpawnRadius;
    public float maxSpawnRadius;
    public int minClusterSize;
    public int maxClusterSize;
    public float spawnRate;
    public int maxEnemies;
    private int enemyCount;
    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = Player.Instance.gameObject;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (enemyCount < maxEnemies)
            {
                int packSize = UnityEngine.Random.Range(minClusterSize, maxClusterSize + 1);
                Vector3 clusterCenter = player.transform.position
                    + Random.insideUnitSphere * UnityEngine.Random.Range(minSpawnRadius, maxSpawnRadius);
                clusterCenter.y = 0f;

                for (int i = 0; i < packSize && enemyCount < maxEnemies; i++)
                {
                    Vector3 spawnPosition = clusterCenter + Random.insideUnitSphere * 2f;
                    spawnPosition.y = 0f;
                    SpawnEnemy(spawnPosition);
                    enemyCount++;
                }
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
    private void SpawnEnemy(Vector3 position)
    {
        GameObject enemy = ObjectPooler.Instance.SpawnFromPool(enemyPrefab, position, Quaternion.identity);
    }
}