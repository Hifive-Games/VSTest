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
            if (collider.TryGetComponent(out CharacterController player) && (Caster == Caster.Enemy || Caster == Caster.Boss))
            {
                //Damage
                Debug.Log($"Player hit by {Name} spell!\nDamage: {damage}");
            }
        }
    }
    public override void CollisionEffect(Enemy enemy)
    {
        if (enemy != null) enemy.TakeDamage(damage);
        Explode();
        base.CollisionEffect(enemy);
    }

    public override void CollisionEffect(CharacterController player)
    {
        if (player != null) Explode();
        base.CollisionEffect(player);
    }
}
