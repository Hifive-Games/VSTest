using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    private const int POOL_SIZE = 5000;

    [SerializeField] private GameObject bulletPoolHolder;
    [SerializeField] private GameObject enemyPoolHolder;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject experiancePrefab;

    private void Start()
    {
        CreateObjectPool(enemyPrefab, enemyPoolHolder.transform);
        CreateObjectPool(bulletPrefab,bulletPoolHolder.transform);
        CreateObjectPool(experiancePrefab, enemyPoolHolder.transform);
    }

    private void CreateObjectPool(GameObject prefab, Transform parent)
    {
        ObjectPooler.Instance.CreatePool(prefab, POOL_SIZE, parent);
    }
}