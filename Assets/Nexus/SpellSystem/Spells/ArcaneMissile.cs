using System.Collections;
using UnityEngine;

public class ArcaneMissile : Spell
{
    // tuning
    public float seekerSpeed = 10f;    // how fast the missile moves
    public float lifetime     = 2f;     // how long it seeks

    private float lifeTimer;
    private Transform currentTarget;

    public override void Seek(Transform target = null)
    {

    }

    public override void Release()
    {
        SeekAndHit();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        lifetime = duration;
        seekerSpeed = speed;
        lifeTimer = 0f;
        currentTarget = null;
        StartCoroutine(SeekAndHit());
    }

    public override void OnDisable()
    {
        StopAllCoroutines();
        base.OnDisable();
    }

    private IEnumerator SeekAndHit()
    {
        while (lifeTimer < lifetime)
        {
            // if we have no target or it’s already been hit, find a fresh one
            if (currentTarget == null ||
                !currentTarget.TryGetComponent<Enemy>(out var e) ||
                 e.hitBySpell)
            {
                currentTarget = FindClosestEnemy();
                if (currentTarget == null) break;
            }

            // move toward it
            Vector3 dir = (currentTarget.position - transform.position).normalized;
            transform.position += dir * seekerSpeed * Time.deltaTime;
            lifeTimer += Time.deltaTime;

            // if close enough, hit and force a new target next loop
            if (Vector3.Distance(transform.position, currentTarget.position) < 0.5f)
            {
                if (currentTarget.TryGetComponent<Enemy>(out e))
                {
                    e.TakeDamage(damage, DamageNumberType.Spell);
                    e.hitBySpell = true;
                }
                currentTarget = null;
            }

            yield return null;
        }

        // done seeking—return to pool
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    public override Transform FindClosestEnemy()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hits = Physics.OverlapSphere(transform.position, range, mask);

        Transform closest = null;
        float bestDist = range;
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Enemy>(out var e) && !e.hitBySpell)
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    closest = hit.transform;
                }
            }
        }
        return closest;
    }

    public override void CollisionEffect(Enemy enemy = null)
    {
        // do nothing
    }
}