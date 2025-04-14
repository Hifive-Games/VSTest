using UnityEngine;

public class ArcaneMissile : Spell
{
    public GameObject child;
    public int childCount = 3;
    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public void Split()
    {
        //split into multiple projectiles
        for (int i = 0; i < childCount; i++)
        {
            GameObject _child = ObjectPooler.Instance.SpawnFromPool(child, transform.position, Quaternion.identity);

            ArcaneMissileChild missile = _child.GetComponent<ArcaneMissileChild>();
            missile.Initialize(TheHero.Instance.transform, damage / childCount);
        }


        base.CollisionEffect();
    }

    public override void CollisionEffect(Enemy enemy)
    {
        if (enemy.hitBySpell) Seek(FindClosestEnemy());
        else
        {
            enemy.TakeDamage(damage);
            enemy.hitBySpell = true;
            Split();
        }
    }

    public override void CollisionEffect(Player player)
    {
        if (player != null) player.TakeDamage(damage);
        base.CollisionEffect(player);
    }

    public override Transform FindClosestEnemy()
    {

        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hits = Physics.OverlapSphere(transform.position, range, mask);

        Transform closest = null;
        float closestDistance = range;
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
}
