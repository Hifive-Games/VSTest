using System;
using System.Collections;
using UnityEngine;
using Zenject.SpaceFighter;

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

    public override void Release()
    {
        // Play fireball sound effect
        SFXManager.Instance.PlayAt(SFX.Fireball);
        base.Release();
    }

    public void Explode()
    {
        ObjectPooler.Instance.SpawnFromPool(ExplosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius/2f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy) && Caster == Caster.Player)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
    public override void CollisionEffect(Enemy enemy)
    {
        if (enemy != null) enemy.TakeDamage(damage);
        Explode();
        // Play explosion sound effect
        SFXManager.Instance.PlayAt(SFX.FireballExplosion);
        base.CollisionEffect(enemy);
    }
}
