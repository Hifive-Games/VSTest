using UnityEngine;

public class GarlicSpell : Spell
{

    float tickTimer;

    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }

    private void Update()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0)
            {
                DamageNearbyEnemies();
                tickTimer = tickInterval;
            }
        }
        else
        {
            ObjectPooler.Instance.ReturnObject(gameObject);
        }


        transform.position = Player.Instance.transform.position;

        //rotate the garlic
        transform.Rotate(Vector3.up, 360 * Time.deltaTime);
    }

    void DamageNearbyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        if (colliders.Length == 0) return;
        foreach (Collider c in colliders)
        {
            if (c.TryGetComponent(out Enemy e))
                e.TakeDamage(damage);
        }
    }

    public override void CollisionEffect(Enemy enemy)
    {
    }
}