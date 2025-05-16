
using UnityEngine;

public class Fireball : Spell
{
    [SerializeField] GameObject ExplosionEffect;

    private Collider[] _hitBuffer = new Collider[16]; // Reusable buffer for overlap sphere

    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void Release()
    {
        // Play fireball sound effect
        SFXManager.Instance.PlayAt(SFX.Fireball);
        base.Release();
    }

    public void Explode()
    {
        ObjectPooler.Instance.SpawnFromPool(ExplosionEffect, transform.position, Quaternion.identity);
        int hits = Physics.OverlapSphereNonAlloc(transform.position, radius / 2, _hitBuffer);
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
    public override void CollisionEffect(Enemy enemy)
    {
        Explode();
        // Play explosion sound effect
        SFXManager.Instance.PlayAt(SFX.FireballExplosion);
        base.CollisionEffect(enemy);
    }
}
