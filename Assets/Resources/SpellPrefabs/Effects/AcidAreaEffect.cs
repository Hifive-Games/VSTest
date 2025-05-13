using System.Collections;
using UnityEngine;

public class AcidAreaEffect : MonoBehaviour
{
    private int damage;
    private float radius;
    private float tickInterval;
    private float duration;
    private static Collider[] _hitBuffer = new Collider[16]; // Reusable buffer for overlap sphere
    //[SerializeField] private int armorReduction = 10; // Armor reduction value

    private float tickTimer;

    private Caster caster;

    public void Initialize(int damage, float radius, float tickInterval, float duration, Caster caster)
    {
        this.damage = damage;
        this.radius = radius;
        this.tickInterval = tickInterval;
        this.duration = duration;
        tickTimer = tickInterval;
        this.caster = caster;

        StartCoroutine(AcidEffect());
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator AcidEffect()
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            tickTimer -= Time.deltaTime;

            if (tickTimer <= 0)
            {
                DamageEnemies();
                tickTimer = tickInterval;
            }

            yield return null;
        }

        // Return acid area to the pool
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    private void DamageEnemies()
    {
        int hits = Physics.OverlapSphereNonAlloc(transform.position, radius /2, _hitBuffer);
        Collider[] colliders = new Collider[hits];
        for (int i = 0; i < hits; i++)
        {
            colliders[i] = _hitBuffer[i];
        }

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // Apply damage
                enemy.TakeDamage(damage, DamageNumberType.Spell);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
