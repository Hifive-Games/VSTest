using System.Collections;
using UnityEngine;

public class BossAttack : MonoBehaviour, IBossAttacker
{
    [Header("Attack Info")]
    [SerializeField] AttackInfo[] attackInfos = new AttackInfo[16];

    // reusable buffer to avoid allocations
    const int MAX_HITS = 16;
    static Collider[] _hitBuffer = new Collider[MAX_HITS];

    public void AddAttack(int attackId,
                          GameObject warningPrefab,
                          float attackDelay,
                          GameObject attackPrefab,
                          float attackDuration,
                          int attackCount,
                          float attackDamage,
                          float attackRange,
                          float attackArea)
    {
        if (attackId < 0 || attackId >= attackInfos.Length)
        {
            Debug.LogError($"Invalid attack ID: {attackId}");
            return;
        }

        attackInfos[attackId] = new AttackInfo
        {
            attackId = attackId,
            attackWarningPrefab = warningPrefab,
            attackDelay = attackDelay,
            attackPrefab = attackPrefab,
            attackDuration = attackDuration,
            attackCount = Mathf.Max(1, attackCount),
            attackDamage = attackDamage,
            attackRange = attackRange,
            attackArea = attackArea
        };
    }
    public void RemoveAttack(int attackId)
    {
        if (attackId < 0 || attackId >= attackInfos.Length)
        {
            Debug.LogError($"Invalid attack ID: {attackId}");
            return;
        }

        attackInfos[attackId] = new AttackInfo();
    }
    public int AttackCount => attackInfos.Length;

    public void DoAttack(int attackId, Transform origin)
    {
        if (attackId < 0 || attackId >= attackInfos.Length)
        {
            Debug.LogError($"Invalid attack ID: {attackId}");
            return;
        }

        StartCoroutine(AttackCoroutine(attackInfos[attackId], origin));
    }

    private IEnumerator AttackCoroutine(AttackInfo info, Transform origin)
    {
        //check the range, if it 0, its around the boss if not, its around the player
        Vector3 center = origin.position;
        if (info.attackRange <= 0f)
        {
            center = transform.position;
        }
        else
        {
            Vector3 dir = (origin.position - transform.position).normalized;
            center = transform.position + dir * info.attackRange;
        }
        center.y = 0.1f; // ignore y axis

        // 1) Warn
        GameObject warnInst = null;
        if (info.attackWarningPrefab != null)
            warnInst = Instantiate(info.attackWarningPrefab, center, Quaternion.identity);
        warnInst.transform.localScale = new Vector3(info.attackArea, info.attackArea, info.attackArea);

        Destroy(warnInst, info.attackDelay);

        yield return new WaitForSeconds(info.attackDelay);

        // 2) Spawn attack VFX
        GameObject atkInst = null;
        if (info.attackPrefab != null)
            atkInst = Instantiate(info.attackPrefab, center, Quaternion.identity);
        atkInst.transform.localScale = new Vector3(info.attackArea, info.attackArea, info.attackArea);

        // 3) Deal damage in `attackCount` evenly spaced ticks
        float tick = info.attackDuration / info.attackCount;
        for (int i = 0; i < info.attackCount; i++)
        {
            DealDamage(center, info.attackArea, info.attackDamage);
            yield return new WaitForSeconds(tick);
        }

        if (atkInst != null)
            Destroy(atkInst);
    }

    private void DealDamage(Vector3 center, float range, float damage)
    {
        int hits = Physics.OverlapSphereNonAlloc(center, range / 2, _hitBuffer);
        for (int i = 0; i < hits; i++)
        {
            var col = _hitBuffer[i];
            if (col != null && col.TryGetComponent<TheHeroDamageManager>(out var damageManager))
            {
                damageManager.TakeDamage(damage);
                // TODO: actually apply damage to player here but we taking 2 hits Should fix this
            }
        }
    }
}

[System.Serializable]
public struct AttackInfo
{
    public int attackId;
    public float attackDelay;
    public float attackDuration;
    public int attackCount;
    public float attackDamage;
    public float attackRange;
    public float attackArea;
    public GameObject attackWarningPrefab;
    public GameObject attackPrefab;
}