using UnityEngine;

public class Fireball : Spell
{
    [SerializeField] GameObject ExplosionEffect;

    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        GameObject explosion = ObjectPooler.Instance.SpawnFromPool(ExplosionEffect, transform.position, ExplosionEffect.transform.rotation);
    }
    public override void CollisionEffect(Enemy enemy)
    {
        Explode();
        base.CollisionEffect(enemy);
    }
}
