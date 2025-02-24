using System;
using System.Collections;
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
        ObjectPooler.Instance.SpawnFromPool(ExplosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy) && Caster == Caster.Player)
            {
                enemy.TakeDamage(damage);
            }
            if (collider.TryGetComponent(out Player player) && (Caster == Caster.Enemy || Caster == Caster.Boss))
            {
                player.TakeDamage(damage);
            }
        }
    }
    public override void CollisionEffect(Enemy enemy)
    {
        if (enemy != null) enemy.TakeDamage(damage);
        Explode();
        base.CollisionEffect(enemy);
    }

    public override void CollisionEffect(Player player)
    {
        if (player != null) Explode();
        base.CollisionEffect(player);
    }
}
