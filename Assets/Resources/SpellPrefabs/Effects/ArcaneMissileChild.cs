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
        while (target != null && lifetime > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                if (target.TryGetComponent(out Enemy enemy) && !enemy.hitBySpell)
                {
                    enemy.TakeDamage(damage);
                    ObjectPooler.Instance.ReturnObject(gameObject);
                }
                else
                {
                    print("Target has already been hit by the spell");
                    Seek();
                }
                yield break;
            }

            yield return null;
        }

        // If target is lost, start rotating around caster
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
