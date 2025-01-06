using System;
using Unity.VisualScripting;
using UnityEngine;

public static class EnemyFactory
{
    public static GameObject CreateEnemy(EnemyDataSO data, Vector3 position)
    {
        Quaternion rotation = Quaternion.identity;
        GameObject prefab = data.enemyPrefab;
        GameObject enemyObj = null;

        Color color = Color.white;

        enemyObj = ObjectPooler.Instance.SpawnFromPool(prefab, position, rotation);


        Debug.Log("EnemyFactory.CreateEnemy: " + data.enemyType);
        switch (data.enemyType)
        {
            case EnemyType.BasicMelee:
                color = Color.red;
                enemyObj.AddComponent<BasicMeleeEnemy>();
                break;
            case EnemyType.Ranged:
                color = Color.blue;
                enemyObj.AddComponent<RangedEnemy>();
                break;
            case EnemyType.FastFragile:
                color = Color.green;
                enemyObj.AddComponent<FastFragileEnemy>();
                break;
            case EnemyType.SlowHardHitter:
                color = Color.yellow;
                enemyObj.AddComponent<SlowHardHitterEnemy>();
                break;
        }


        enemyObj.GetComponent<Enemy>().Initialize(data);
        enemyObj.GetComponentInChildren<Renderer>().material.color = color;

        return enemyObj;
    }
}