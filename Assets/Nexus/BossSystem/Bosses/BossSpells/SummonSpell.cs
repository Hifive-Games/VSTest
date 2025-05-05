using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Boss/Spell/Summon", fileName = "NewSummonSpell")]
public class SummonSpell : BossSpell
{
    public GameObject bfeoreSpawnFX;
    public EnemyDataSO minionPrefab;
    public int quantity = 3;
    public float radius = 5f;

    public override void Cast(BossController boss, Transform target)
    {
        if (minionPrefab == null)
        {
            Debug.LogError("Minion prefab is not assigned in SummonSpell.");
            return;
        }
        if (boss == null)
        {
            Debug.LogError("Boss is not assigned in SummonSpell.");
            return;
        }

        SpawnMinions(boss);
        CastChainSpell(boss, target);
    }

    private void SpawnMinions(BossController boss)
    {
        // kick off the FX+spawn coroutine on the boss MonoBehaviour
        boss.StartCoroutine(SpawnMinionsCoroutine(boss));
    }

    private IEnumerator SpawnMinionsCoroutine(BossController boss)
    {
        // pick positions and play FX
        var spawnPositions = new List<Vector3>();
        for (int i = 0; i < quantity; i++)
        {
            Vector3 pos = boss.transform.position + Random.insideUnitSphere * radius;
            pos.y = 0.1f;
            spawnPositions.Add(pos);

            if (bfeoreSpawnFX != null)
                Instantiate(bfeoreSpawnFX, pos, Quaternion.identity);
        }

        // wait for the spell’s cast time (inherited from BossSpell)
        yield return new WaitForSeconds(castTime);

        // spawn actual minions
        foreach (var pos in spawnPositions)
        {
            EnemySpawner.Instance.SpawnEnemy(minionPrefab, pos);
        }
    }
}