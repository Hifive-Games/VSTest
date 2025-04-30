using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler
{
    private static ObjectPooler instance;
    public static ObjectPooler Instance
    {
        get
        {
            if (instance == null)
                instance = new ObjectPooler();
            return instance;
        }
    }

    private static Dictionary<GameObject, int> prefabToId = new Dictionary<GameObject, int>();
    private static int nextId = 0;

    private struct PoolData
    {
        public Queue<GameObject> queue;
        public Transform parent;
        public GameObject prefab;
        public int poolSize;
    }

    private Dictionary<int, PoolData> poolInfo = new Dictionary<int, PoolData>();

    private int GetPrefabId(GameObject prefab)
    {
        if (prefab == null)
        {
            throw new System.ArgumentNullException("Prefab cannot be null");
        }

        if (!prefabToId.ContainsKey(prefab))
        {
            prefabToId[prefab] = nextId++;
        }
        return prefabToId[prefab];
    }

    public void CreatePool(GameObject prefab, int poolSize, Transform parent)
    {
        int poolKey = GetPrefabId(prefab);
        if (!poolInfo.ContainsKey(poolKey))
        {
            var data = new PoolData
            {
                queue = new Queue<GameObject>(poolSize),
                parent = parent,
                prefab = prefab,
                poolSize = poolSize
            };

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = GameObject.Instantiate(prefab, parent);
                newObject.SetActive(false);
                AddPooledObjectComponent(newObject, prefab);
                data.queue.Enqueue(newObject);
            }
            poolInfo.Add(poolKey, data);
        }
    }

    private void AddPooledObjectComponent(GameObject obj, GameObject prefab)
    {
        PooledObject pooledObject = obj.GetComponent<PooledObject>();
        if (pooledObject == null)
        {
            pooledObject = obj.AddComponent<PooledObject>();
        }
        pooledObject.prefab = prefab;
    }

    public GameObject GetObject(GameObject prefab)
    {
        int poolKey = GetPrefabId(prefab);
        if (poolInfo.TryGetValue(poolKey, out PoolData data))
        {
            if (data.queue.Count == 0)
            {
                // Remove or limit this expansion if needed
                GameObject newObject = GameObject.Instantiate(data.prefab, data.parent);
                newObject.SetActive(false);
                AddPooledObjectComponent(newObject, prefab);
                data.queue.Enqueue(newObject);
            }
            GameObject objectToSpawn = data.queue.Dequeue();

            poolInfo[poolKey] = data;
            return objectToSpawn;
        }
        return null;
    }

    public void ReturnObject(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        PooledObject pooledObject = objectToReturn.GetComponent<PooledObject>();
        if (pooledObject != null)
        {
            GameObject prefab = pooledObject.prefab;
            int poolKey = GetPrefabId(prefab);
            if (poolInfo.TryGetValue(poolKey, out PoolData data))
            {
                data.queue.Enqueue(objectToReturn);
                poolInfo[poolKey] = data;
            }
        }
    }

    public void ClearPool(GameObject prefab)
    {
        int poolKey = GetPrefabId(prefab);
        if (poolInfo.TryGetValue(poolKey, out PoolData data))
        {
            foreach (GameObject obj in data.queue)
            {
                GameObject.Destroy(obj);
            }
            data.queue.Clear();
            poolInfo.Remove(poolKey);
        }
    }

    public void ClearAllPools()
    {
        foreach (var kvp in poolInfo)
        {
            foreach (GameObject obj in kvp.Value.queue)
            {
                GameObject.Destroy(obj);
            }
        }
        poolInfo.Clear();
        prefabToId.Clear();
        nextId = 0;
    }

    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = GetObject(prefab);
        if (objectToSpawn != null)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);
        }
        return objectToSpawn;
    }

    public GameObject SpawnFromPool(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        if (prefab != null)
        {
            return SpawnFromPool(prefab, position, rotation);
        }
        Debug.LogError($"Prefab {prefabName} not found in Resources folder.");
        return null;
    }

    public GameObject SpawnFromPool(string prefabName, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        if (prefab != null)
        {
            GameObject objectToSpawn = SpawnFromPool(prefab, position, rotation);
            if (objectToSpawn != null && parent != null)
            {
                objectToSpawn.transform.SetParent(parent);
            }
            return objectToSpawn;
        }
        Debug.LogError($"Prefab {prefabName} not found in Resources folder.");
        return null;
    }
}
public class PooledObject : MonoBehaviour
{
    public GameObject prefab;
}