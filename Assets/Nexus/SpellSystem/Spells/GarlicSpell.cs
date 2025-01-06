using UnityEngine;

public class GarlicSpell : Spell
{

    float tickTimer;

    float _duration = 5f;

    public override void OnEnable()
    {
        _duration = duration;
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

    public override void OnTriggerEnter(Collider other)
    {
    }
}