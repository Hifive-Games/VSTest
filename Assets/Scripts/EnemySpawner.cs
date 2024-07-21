using Unity.Mathematics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;

    public int enemyCount = 0;
    public int maxEnemies = 10;
    public float spawnRate = 1f;
    private float nextSpawn = 0f;

    public float minSpawnRadius = 5f;
    public float maxSpawnRadius = 10f;
    public bool spawn = true;

    public int killCount = 0;

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
    }

    public Vector3 GetRandomSpawnPositionFromPlayer()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 randomPosition = player.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);
        return randomPosition;
    }

    private void Update()
    {
        if (spawn && Time.time > nextSpawn && enemyCount < maxEnemies)
        {
            nextSpawn = Time.time + spawnRate;
            SpawnEnemy();

            InterfaceManager.Instance.UpdateEnemyCount(enemyCount,maxEnemies);
        }
    }

    public void IncreaseSpwanRate()
    {
        spawnRate /= 1.01f;
        maxEnemies = (int)math.ceil(maxEnemies * 1.01f);

        if(spawnRate <= 0.001f) spawnRate = 0.001f;
    }

    private void SpawnEnemy()
    {
        GameObject enemy = ObjectPooler.Instance.GetObject(enemyPrefab);
        enemy.transform.position = GetRandomSpawnPositionFromPlayer();
        enemyCount++;
    }

    public Vector3 GetMiddilePointOfTheScreen()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, minSpawnRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.transform.position, maxSpawnRadius);
    }
}
