using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [System.Serializable]
    private struct PoolItem
    {
        public GameObject prefab;
        public Transform parent;
        public int size;
    }

    public const int DEFAULT_POOL_SIZE = 10;
    [SerializeField]
    private List<PoolItem> itemsToPool = new List<PoolItem>
    {
        new PoolItem { prefab = null, parent = null, size = 0 }
    };

    private void Start()
    {
        LoadObjects();
        CreatePools();
    }

    void LoadObjects()
    {
        List<GameObject> loadedObjects = new List<GameObject>();
        GameObject _parent = new GameObject("LoadedPoolObjects");
        _parent.transform.SetParent(transform);
        foreach (GameObject item in Resources.LoadAll("Objects"))
        {
            loadedObjects.Add(item);
        }

        if (loadedObjects.Count == 0)
        {
            Debug.LogWarning("No objects found in Resources/Objects folder");
            return;
        }

        foreach (var item in loadedObjects)
        {
            itemsToPool.Add(new PoolItem { prefab = item, parent = _parent.transform , size = DEFAULT_POOL_SIZE });
        }
    }

    public void CreatePools()
    {
        foreach (var item in itemsToPool)
        {
            if (item.prefab != null && item.parent != null)
            {
                ObjectPooler.Instance.CreatePool(item.prefab, item.size, item.parent);
            }
        }
    }
}