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

    [SerializeField] private List<PoolItem> itemsToPool = new List<PoolItem>
    {
        new PoolItem { prefab = null, parent = null, size = 0 }
    };

    private void Start()
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