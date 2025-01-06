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
    }
    public override void CollisionEffect(Enemy enemy)
    {
        Explode();
        base.CollisionEffect(enemy);
    }
}
