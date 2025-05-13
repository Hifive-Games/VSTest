using System.Collections;
using UnityEngine;

public class ArcaneMissileChild : MonoBehaviour
{
    private float speed = 10f;
    private int damage = 10;
    private float lifetime = 2f;
    public Transform casterTransform;
    private Transform target;

    public void Initialize(Transform casterTransform, int damage)
    {
        this.casterTransform = casterTransform;
        this.damage = damage;
    }

    void OnEnable()
    {
        StartCoroutine(DestroyAfterLifetime());
        Seek();
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    // Missile seeks a valid target; otherwise, it rotates around the caster
    public void Seek()
    {
        target = FindClosestEnemy();
        if (target != null)
        {
            StartCoroutine(MoveToTarget());
        }
        else
        {
            StartCoroutine(RotateAroundCaster());
        }
    }

    private Transform FindClosestEnemy()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hits = Physics.OverlapSphere(transform.position, 10f, mask);

        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Enemy enemy) && enemy.hitBySpell)
                continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = hit.transform;
            }
        }

        return closest;
    }

    private IEnumerator MoveToTarget()
    {
        // keep going until child’s lifetime runs out
        while (lifetime > 0f)
        {
            if (target != null)
            {
                // move toward current target
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

                // if close enough, apply damage and immediately pick a new target
                if (Vector3.Distance(transform.position, target.position) < 0.5f)
                {
                    if (target.TryGetComponent(out Enemy enemy) && !enemy.hitBySpell)
                    {
                        // deal damage and mark so no other missile hits it
                        enemy.TakeDamage(damage, DamageNumberType.Spell);
                        enemy.hitBySpell = true;

                        // look for the next closest enemy
                        target = FindClosestEnemy();
                        if (target != null)
                        {
                            // loop back and chase the new target
                            yield return null;
                            continue;
                        }
                    }
                    // either no valid enemy left or it was already hit
                    break;
                }
            }
            yield return null;
        }

        // no more targets (or lifetime expired), switch to orbiting until lifetime runs out
        StartCoroutine(RotateAroundCaster());
    }

    private IEnumerator RotateAroundCaster()
    {
        float radius = 1.5f; // Orbit radius around caster
        float angle = 0f;

        while (lifetime > 0)
        {
            if (casterTransform == null)
                yield break;

            angle += 180 * Time.deltaTime; // Rotate speed
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            transform.position = casterTransform.position + offset;

            yield return null;
        }

        ObjectPooler.Instance.ReturnObject(gameObject);
    }
}
