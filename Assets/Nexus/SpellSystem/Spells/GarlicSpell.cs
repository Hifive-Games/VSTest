using UnityEngine;

public class GarlicSpell : Spell
{

    float tickTimer;

    public override void OnEnable()
    {
    }

    public override void Release()
    {
        StopAllCoroutines();
    }

    public override void Seek(Transform target = null)
    {
        //no need to seek, garlic will follow the player
    }

    public override void OnDisable()
    {
        //no need to disable garlic, it will follow the player
    }

    private void Update()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0)
        {
            DamageNearbyEnemies();
            tickTimer = tickInterval;
        }

        //return the garlic when duration is over
        if (duration > 0)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                ObjectPooler.Instance.ReturnObject(gameObject);
            }
        }


        transform.position = FollowCasterTransform().position;

        //rotate the garlic
        transform.Rotate(Vector3.up, 360 * Time.deltaTime);
    }

    public Transform FollowCasterTransform()
    {
        return TheHero.Instance.transform;
    }

    void DamageNearbyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius / 2f);
        foreach (Collider c in colliders)
        {
            if (c.TryGetComponent(out Enemy e) && Caster == Caster.Player)
                e.TakeDamage(damage);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
    }
}