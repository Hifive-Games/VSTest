using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkManager : MonoBehaviour
{
    [Header("Chunk Settings")]
    public GameObject chunkPrefab;
    public GameObject[] objectPrefabs;
    public GameObject[] altarPrefabs;
    public GameObject bossAltarPrefab;
    public int chunkSize = 16;

    private Vector2Int currentChunk;
    private Vector2Int startingChunk;

    [Header("Object Settings")]
    public int minObjects = 3;
    public int maxObjects = 8;

    [Range(0, 100)]
    public int altarChance = 5;
    public int bossAltarSpawnDistance = 5; // Boss altar her 5 chunk'ta bir

    [Header("Boss Settings")]
    public GameObject bossArenaPrefab;
    public GameObject bossPrefab;

    [Header("Player Settings")]
    public Transform player;

    [Header("Random Settings")]
    public bool useSeed = false;
    public int seed = 0;

    private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, List<(GameObject prefab, Vector3 position, Quaternion rotation)>> chunkData
        = new Dictionary<Vector2Int, List<(GameObject, Vector3, Quaternion)>>();
    private HashSet<Vector2Int> bossAltarChunks = new HashSet<Vector2Int>();

    public event Action<Vector3> OnBossAltarActivated;

    void Start()
    {
        Invoke(nameof(Initialize), 0.1f);
    }

    public void Initialize()
    {
        player = FindAnyObjectByType<CharacterController>().transform;
        objectPrefabs = Resources.LoadAll<GameObject>("Objects");
        if (useSeed) Random.InitState(seed);

        currentChunk = GetChunkCoord(player.position);
        startingChunk = currentChunk;                 // <- capture start
        GenerateInitialChunks();
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }
        Vector2Int newChunk = GetChunkCoord(player.position);
        if (newChunk != currentChunk)
        {
            currentChunk = newChunk;
            UpdateChunks();
        }
    }

    Vector2Int GetChunkCoord(Vector3 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x / chunkSize), Mathf.FloorToInt(position.z / chunkSize));
    }

    void GenerateInitialChunks()
    {
        Debug.Log("Generating initial chunks...");
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int chunkCoord = new Vector2Int(currentChunk.x + x, currentChunk.y + y);
                SpawnChunk(chunkCoord);
            }
        }
    }

    void UpdateChunks()
    {
        HashSet<Vector2Int> newActiveChunks = new HashSet<Vector2Int>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int coord = new Vector2Int(currentChunk.x + x, currentChunk.y + y);
                if (!activeChunks.ContainsKey(coord))
                {
                    SpawnChunk(coord);
                }
                newActiveChunks.Add(coord);
            }
        }

        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (var chunk in activeChunks.Keys)
        {
            if (!newActiveChunks.Contains(chunk))
            {
                chunksToRemove.Add(chunk);
            }
        }

        foreach (var chunkCoord in chunksToRemove)
        {
            if (activeChunks.TryGetValue(chunkCoord, out GameObject chunkObj))
            {
                for (int i = chunkObj.transform.childCount - 1; i >= 0; i--)
                {
                    GameObject child = chunkObj.transform.GetChild(i).gameObject;
                    ObjectPooler.Instance.ReturnObject(child);
                }
                ObjectPooler.Instance.ReturnObject(chunkObj);
                activeChunks.Remove(chunkCoord);
            }
        }
    }

    void SpawnChunk(Vector2Int chunkCoord)
    {
        GameObject newChunk = ObjectPooler.Instance.SpawnFromPool(
            chunkPrefab,
            new Vector3(chunkCoord.x * chunkSize, 0f, chunkCoord.y * chunkSize),
            Quaternion.identity
        );

        if (newChunk != null)
        {
            newChunk.name = $"Chunk ({chunkCoord.x}, {chunkCoord.y})";
            newChunk.transform.parent = transform;

            if (!chunkData.ContainsKey(chunkCoord))
            {
                chunkData[chunkCoord] = GenerateObjectData(chunkCoord);
            }

            foreach (var data in chunkData[chunkCoord])
            {
                GameObject pooledObj = ObjectPooler.Instance.SpawnFromPool(data.prefab, data.position, data.rotation);
                if (pooledObj != null)
                {
                    pooledObj.transform.SetParent(newChunk.transform);
                }
            }

            activeChunks[chunkCoord] = newChunk;
        }
    }

    List<(GameObject, Vector3, Quaternion)> GenerateObjectData(Vector2Int chunkCoord)
    {
        if (chunkCoord == startingChunk)
            return new List<(GameObject, Vector3, Quaternion)>();

        Vector3 chunkOrigin = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);
        List<(GameObject, Vector3, Quaternion)> list = new List<(GameObject, Vector3, Quaternion)>();
        int objectCount = Random.Range(minObjects, maxObjects + 1);
        HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();

        // Boss altar check (still excludes the whole 3×3 initial)
        bool isInitialChunk = Mathf.Abs(chunkCoord.x) <= 1 && Mathf.Abs(chunkCoord.y) <= 1;
        bool isBossAltarChunk = !isInitialChunk
                                && (chunkCoord.x % bossAltarSpawnDistance == 0)
                                && (chunkCoord.y % bossAltarSpawnDistance == 0);

        //Todo: Boss altar spawn kontrolü şimdilik kapalı, ileride açılacak

        isBossAltarChunk = false;

        if (isBossAltarChunk)
        {
            bossAltarChunks.Add(chunkCoord);
            Vector2Int bossPos = new Vector2Int(0, 0);
            occupiedPositions.Add(bossPos);

            Vector3 worldPos = chunkOrigin + new Vector3(0, 1, 0);
            list.Add((bossAltarPrefab, worldPos, Quaternion.identity));

            Debug.Log($"Boss altar spawned at chunk ({chunkCoord.x}, {chunkCoord.y})");
        }
        else
        {
            // 🌿 Normal altar spawn kontrolü
            bool hasAltar = UnityEngine.Random.Range(0, 100) < altarChance;
            if (hasAltar)
            {
                Vector2Int altarPos = new Vector2Int(UnityEngine.Random.Range(-chunkSize / 4, chunkSize / 4), UnityEngine.Random.Range(-chunkSize / 4, chunkSize / 4));
                occupiedPositions.Add(altarPos);

                GameObject altarPrefab = altarPrefabs[UnityEngine.Random.Range(0, altarPrefabs.Length)];
                Quaternion altarRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
                Vector3 altarWorldPos = chunkOrigin + new Vector3(altarPos.x, 1, altarPos.y);
                list.Add((altarPrefab, altarWorldPos, altarRotation));
            }

            // 🌿 Normal objeleri ekle
            for (int i = 0; i < objectCount; i++)
            {
                Vector2Int randomPos;
                do
                {
                    randomPos = new Vector2Int(UnityEngine.Random.Range(-chunkSize / 2, chunkSize / 2), UnityEngine.Random.Range(-chunkSize / 2, chunkSize / 2));
                }
                while (occupiedPositions.Contains(randomPos));

                occupiedPositions.Add(randomPos);

                GameObject prefab = objectPrefabs[UnityEngine.Random.Range(0, objectPrefabs.Length)];
                Quaternion rot = Quaternion.Euler(-90, UnityEngine.Random.Range(0, 360), 0);
                Vector3 pos = chunkOrigin + new Vector3(randomPos.x, 0, randomPos.y);

                list.Add((prefab, pos, rot));
            }
        }

        return list;
    }

    public void ActivateBossAltar(Vector3 position)
    {
        OnBossAltarActivated?.Invoke(position);

        Vector2Int chunkCoord = GetChunkCoord(position);
        if (bossAltarChunks.Contains(chunkCoord))
        {
            bossAltarChunks.Remove(chunkCoord);
            if (activeChunks.TryGetValue(chunkCoord, out GameObject chunkObj))
            {
                for (int i = chunkObj.transform.childCount - 1; i >= 0; i--)
                {
                    GameObject child = chunkObj.transform.GetChild(i).gameObject;
                    ObjectPooler.Instance.ReturnObject(child);
                }
            }
        }
    }

    public void SpawnBossArena(Vector3 position)
    {
        Debug.Log($"Boss arena spawned at {position}");
        ObjectPooler.Instance.SpawnFromPool(bossArenaPrefab, position, Quaternion.identity);

        SpawnBoss(position);
    }

    public void SpawnBoss(Vector3 position)
    {
        Debug.Log($"Boss spawned at {position}");

        ObjectPooler.Instance.SpawnFromPool(bossPrefab, position, Quaternion.identity);
    }

    [Button("Clear Chunks")]
    void ClearChunks()
    {
        foreach (var chunk in activeChunks.Values)
        {
            ObjectPooler.Instance.ReturnObject(chunk);
        }
        activeChunks.Clear();
        chunkData.Clear();
    }

    [Button("Generate Random Seed")]
    void GenerateRandomSeed()
    {
        seed = Random.Range(0, int.MaxValue);

        ClearChunks();
        Random.InitState(seed);
        GenerateInitialChunks();
    }

    [Button("Spawn Boss Altar")]
    void SpawnBossAltar()
    {
        Vector2Int chunkCoord = new Vector2Int(currentChunk.x, currentChunk.y);
        if (bossAltarChunks.Contains(chunkCoord))
        {
            return;
        }

        bossAltarChunks.Add(chunkCoord);
        Vector3 chunkOrigin = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);
        Vector3 worldPos = chunkOrigin + new Vector3(0, 1, 0);
        List<(GameObject, Vector3, Quaternion)> data = new List<(GameObject, Vector3, Quaternion)>();
        data.Add((bossAltarPrefab, worldPos, Quaternion.identity));

        foreach (var chunk in activeChunks)
        {
            if (chunk.Key == chunkCoord)
            {
                for (int i = chunk.Value.transform.childCount - 1; i >= 0; i--)
                {
                    GameObject child = chunk.Value.transform.GetChild(i).gameObject;
                    ObjectPooler.Instance.ReturnObject(child);
                }

                foreach (var d in data)
                {
                    GameObject pooledObj = ObjectPooler.Instance.SpawnFromPool(d.Item1, d.Item2, d.Item3);
                    if (pooledObj != null)
                    {
                        pooledObj.transform.SetParent(chunk.Value.transform);
                    }
                }
            }
        }
    }

    [Button("Activate Nearby Boss Altar")]
    void ActivateNearbyBossAltar()
    {
        Vector2Int playerChunk = GetChunkCoord(player.position);
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
        };

        foreach (var dir in directions)
        {
            Vector2Int chunkCoord = playerChunk + dir;
            if (bossAltarChunks.Contains(chunkCoord))
            {
                Vector3 chunkOrigin = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);
                Vector3 worldPos = chunkOrigin + new Vector3(0, 1, 0);
                ActivateBossAltar(worldPos);
                break;
            }
        }

        SpawnBossArena(player.position);
    }
}
