using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    [Header("Phases")]
    public SpawnPhase[] spawnPhases;

    [SerializeField] private float timer;
    [SerializeField] private int groupsSpawned;
    [SerializeField] private int enemiesSpawned;
    [SerializeField] private int maxEnemies;
    [SerializeField] private bool endPhase;
    [SerializeField] private float gatheringDistance;

    public List<GameObject> activeEnemies = new List<GameObject>();
    private GameObject player;
    private Camera mainCamera;

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
        mainCamera = Camera.main;
        player = FindAnyObjectByType<CharacterController>().gameObject;
        StartCoroutine(SpawnEnemies());
        StartCoroutine(GatherEnemies());
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            foreach (var phase in spawnPhases)
            {
                if (timer >= phase.startTime && timer <= phase.endTime && !endPhase)
                {
                    if (groupsSpawned < phase.maxClusterGroupLimit)
                    {
                        yield return SpawnPhaseGroup(phase);
                        groupsSpawned++;
                    }
                    else if (enemiesSpawned < maxEnemies)
                    {
                        SpawnPhase phaseCopy = phase;
                        yield return SpawnPhaseGroup(phaseCopy);
                    }
                    else
                    {
                        Debug.Log("Max enemies spawned - waiting for next phase");
                        yield return new WaitForSeconds(5f);
                    }
                }

                if (timer >= phase.endTime)
                {
                    groupsSpawned = 0;
                    endPhase = true;
                    Debug.Log("End of phase - waiting for next phase");
                    yield return new WaitForSeconds(5f);
                }

                if (endPhase)
                {
                    for (int i = activeEnemies.Count - 1; i >= 0; i--)
                    {
                        GameObject enemy = activeEnemies[i];
                        if (!IsInCullingArea(mainCamera, enemy.transform.position))
                        {
                            ObjectPooler.Instance.ReturnObject(enemy);
                            RemoveEnemy(enemy);
                        }
                    }

                    endPhase = false;

                    Debug.Log("End of phase - Spawning next phase");
                }

                float waitTime = Mathf.Max(phase.spawnInterval - groupsSpawned * .1f, .05f);
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }
    }

    private IEnumerator SpawnPhaseGroup(SpawnPhase phase)
    {
        foreach (var group in phase.enemyGroups)
        {
            Vector3 spawnPos = GetOutsideCameraView();
            for (int i = 0; i < group.amount; i++)
            {
                Vector3 spawnPosCluster;
                float minDistance = group.minSpwanRadius;
                do
                {
                    spawnPosCluster = spawnPos + (Random.insideUnitSphere * group.maxSpawnRadius);
                    spawnPosCluster.y = 1.5f;
                } while (Vector3.Distance(spawnPosCluster, player.transform.position) < minDistance);
                spawnPosCluster.y = 1.5f;
                SpawnEnemy(spawnPosCluster, group.enemyData);
                yield return new WaitForSeconds(.1f);
            }
        }
    }

    private IEnumerator GatherEnemies()
    {
        while (true)
        {
            if (GatherTime())
            {
                TeleportNearestToPlayer();
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private bool GatherTime()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 enemyVolume = Vector2.zero;
        List<GameObject> outOfBounds = OutOfBoundsEnemies();
        foreach (var enemy in outOfBounds)
        {
            Vector2 enemyPos = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
            enemyVolume += enemyPos;
        }
        Vector2 averagePos = enemyVolume / outOfBounds.Count;
        float distance = Vector2.Distance(playerPos, averagePos);
        return distance > gatheringDistance;

    }

    private void SpawnEnemy(Vector3 position, EnemyDataSO enemyData)
    {
        GameObject enemy = EnemyFactory.CreateEnemy(enemyData, position);
        activeEnemies.Add(enemy);
        enemiesSpawned++;
    }

    private Vector3 GetOutsideCameraView()
    {
        const int maxAttempts = 20;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPos = player.transform.position + (Random.insideUnitSphere * 50f);
            randomPos.y = 1.5f;
            if (!IsInCullingArea(mainCamera, randomPos))
            {
                return randomPos;
            }
        }
        Vector3 emergencyPos = player.transform.position + (Random.insideUnitSphere * 70f);
        emergencyPos.y = 1.5f;
        return emergencyPos;
    }

    private void TeleportNearestToPlayer()
    {
        List<GameObject> outOfBounds = OutOfBoundsEnemies();
        foreach (var enemy in outOfBounds)
        {
            Vector3 spawnPos = GetOutsideCameraView();
            enemy.transform.position = spawnPos;
        }
    }

    private List<GameObject> OutOfBoundsEnemies()
    {
        List<GameObject> outOfBounds = new List<GameObject>();
        foreach (var enemy in activeEnemies)
        {
            if (!IsInCullingArea(mainCamera, enemy.transform.position))
            {
                outOfBounds.Add(enemy);
            }
        }
        return outOfBounds;
    }

    private List<GameObject> InBoundsEnemies()
    {
        List<GameObject> inBounds = new List<GameObject>();
        foreach (var enemy in activeEnemies)
        {
            if (IsInCullingArea(mainCamera, enemy.transform.position))
            {
                inBounds.Add(enemy);
            }
        }
        return inBounds;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        enemiesSpawned--;
    }

    public bool IsInCullingArea(Camera cam, Vector3 position)
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(position);
        float offset = 0.1f;
        bool isInTheScreen = screenPoint.x > 0 - offset && screenPoint.x < 1 + offset && screenPoint.y > 0 - offset && screenPoint.y < 1 + offset;
        return isInTheScreen;
    }
}