using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightningBall : Spell
{
    float tickTimer;

    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
    public override void Seek(Transform target = null)
    {
        Vector3 randomPoint = GetRandomPointInRange();
        transform.position = randomPoint;
        StartCoroutine(WaitAndReturnToPool());
    }
    private IEnumerator WaitAndReturnToPool()
    {
        yield return new WaitForSeconds(3f);
        ObjectPooler.Instance.ReturnObject(gameObject);
    }
    private Vector3 GetRandomPointInRange()
    {
        Vector2 randomCircle = Random.insideUnitCircle * range;
        Vector3 randomPoint = new Vector3(randomCircle.x, 1, randomCircle.y);
        return randomPoint;
    }

    public void Update()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0)
        {
            DamageNearbyEnemies();
            tickTimer = tickInterval;
        }
    }
    void DamageNearbyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius/2f);
        foreach (Collider c in colliders)
        {
            if (c.TryGetComponent(out Enemy e) && Caster == Caster.Player)
                e.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius / 2f);
    }

    public override void Release()
    {
        // No specific release logic for this spell
    }
    public override void CollisionEffect(Enemy enemy)
    {
        // No specific collision effect for this spell
    }
    public override void CollisionEffect(CharacterController player)
    {
        // No specific collision effect for this spell
    }
    public override Transform FindClosestEnemy()
    {
        // No specific logic for finding the closest enemy for this spell
        return null;
    }
}
