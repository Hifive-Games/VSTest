using System.Collections;
using UnityEngine;

public class AcidAreaEffect : MonoBehaviour
{
    private int damage;
    private float radius;
    private float tickInterval;
    private float duration;
    [SerializeField] private int armorReduction = 10; // Armor reduction value

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
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy) && caster == Caster.Player && enemy != null)
            {
                enemy.TakeDamage(damage);
                // Apply armor reduction debuff (you'll need to add an ApplyDebuff method to the Enemy class)
                if (enemy.HasDebuff(DebuffType.ArmorReduction))
                {
                    enemy.RemoveDebuff(DebuffType.ArmorReduction);
                }
                enemy.ApplyDebuff(new Debuff
                {
                    Type = DebuffType.ArmorReduction,
                    Value = armorReduction,
                    Duration = tickInterval // Duration of armor reduction per tick
                });
            }

            if (collider.TryGetComponent(out Player player) && caster == Caster.Boss)
            {
                print($"Acid area cast by {caster} collided with {player.name}");
                player.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
