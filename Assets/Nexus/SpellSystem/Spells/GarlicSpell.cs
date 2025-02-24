using UnityEngine;

public class GarlicSpell : Spell
{

    float tickTimer;

    float _duration = 5f;

    public override void OnEnable()
    {
        _duration = duration;
        StopAllCoroutines();
    }

    public override void Release()
    {
    }

    public override void Seek(Transform target = null)
    {
    }

    public override void OnDisable()
    {
    }

    private void Update()
    {
        if (_duration > 0)
        {
            _duration -= Time.deltaTime;
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


        transform.position = FollowCasterTransform().position;

        //rotate the garlic
        transform.Rotate(Vector3.up, 360 * Time.deltaTime);
    }

    public Transform FollowCasterTransform()
    {
        return Caster == Caster.Player ? Player.Instance.transform : ExampleBoss.Instance.transform;
    }

    void DamageNearbyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.TryGetComponent(out Enemy e) && Caster == Caster.Player)
                e.TakeDamage(damage);
            if (c.TryGetComponent(out Player p) && Caster == Caster.Boss)
                p.TakeDamage(damage);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
    }
}