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

    private readonly Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    public void CreatePool(GameObject prefab, int poolSize,Transform parent)
    {
        string poolKey = prefab.name;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = GameObject.Instantiate(prefab,parent);
                newObject.name = poolKey;
                newObject.SetActive(false);
                poolDictionary[poolKey].Enqueue(newObject);
            }
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        string poolKey = prefab.name;

        if (poolDictionary.ContainsKey(poolKey) && poolDictionary[poolKey].Count > 0)
        {
            GameObject objectToSpawn = poolDictionary[poolKey].Dequeue();
            objectToSpawn.SetActive(true);
            return objectToSpawn;
        }

        return null;
    }

    public void ReturnObject(GameObject prefab, GameObject objectToReturn)
    {
        string poolKey = prefab.name;

        if (poolDictionary.ContainsKey(poolKey))
        {
            objectToReturn.SetActive(false);
            poolDictionary[poolKey].Enqueue(objectToReturn);
        }
    }

    public void ClearPool(GameObject prefab)
    {
        string poolKey = prefab.name;

        if (poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary[poolKey].Clear();
        }
    }

    public void ClearAllPools()
    {
        poolDictionary.Clear();
    }

    public void ClearAllPoolsAndDestroy()
    {
        foreach (KeyValuePair<string, Queue<GameObject>> pool in poolDictionary)
        {
            foreach (GameObject obj in pool.Value)
            {
                GameObject.Destroy(obj);
            }
        }

        poolDictionary.Clear();
    }
    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = GetObject(prefab);

        if (objectToSpawn != null)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }

        return objectToSpawn;
    }

    public void ReturnAllObjects(GameObject prefab)
    {
        string poolKey = prefab.name;

        if (poolDictionary.ContainsKey(poolKey))
        {
            foreach (GameObject obj in poolDictionary[poolKey])
            {
                obj.SetActive(false);
            }
        }
    }

}
