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

    public override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (enemy.hitBySpell) Seek(FindClosestEnemy());
            else
            {
                enemy.TakeDamage(damage);
                enemy.hitBySpell = true;
                Split();
            }
        }
    }

    public void Split()
    {
        //split into multiple projectiles
        for (int i = 0; i < childCount; i++)
        {
            GameObject _child = ObjectPooler.Instance.SpawnFromPool(child, transform.position, Quaternion.identity);

            ArcaneMissile childMissile = _child.GetComponent<ArcaneMissile>();
            childMissile.childCount = 0;

            //make the child missile color different
            _child.GetComponent<Renderer>().material.color = Color.blue;
        }


        base.CollisionEffect(null);
    }

    public override Transform FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float closestDistance = Mathf.Min(range, float.MaxValue);
        GameObject closestEnemy = null;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.hitBySpell)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.gameObject;
            }
        }

        if (closestEnemy != null)
        {
            return closestEnemy.transform;
        }
        else
        {
            return null;
        }
    }
}
