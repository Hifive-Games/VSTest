using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public GameObject enemyPrefab;
    public float minSpawnRadius;
    public float maxSpawnRadius;
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
                int packSize = UnityEngine.Random.Range(3, 7);
                Vector3 spawnPosition = GetRandomSpawnPositionFromPlayer();

                for (int i = 0; i < packSize; i++)
                {
                    if (enemyCount >= maxEnemies) break;

                    Vector3 scatterOffset = new Vector3(
                        UnityEngine.Random.Range(-1f, 1f),
                        0,
                        UnityEngine.Random.Range(-1f, 1f)
                    );

                    SpawnEnemy(spawnPosition + scatterOffset);
                    yield return new WaitForSeconds(0.1f); // Small delay between each enemy in the pack
                }

                InterfaceManager.Instance.UpdateEnemyCount(enemyCount, maxEnemies);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    public Vector3 GetRandomSpawnPositionFromPlayer()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 randomPosition = player.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);
        return randomPosition;
    }

    public void IncreaseSpawnRate()
    {
        spawnRate /= 1.01f;
        maxEnemies = (int)Mathf.Ceil(maxEnemies + 2);

        if (spawnRate <= 0.001f) spawnRate = 0.001f;
    }

    private void SpawnEnemy(Vector3 position)
    {
        GameObject enemy = ObjectPooler.Instance.SpawnFromPool(enemyPrefab, position, Quaternion.identity);
        enemyCount++;
    }
}