using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightningBall : Spell
{
    float tickTimer;

    private Collider[] _hitBuffer = new Collider[16]; // Reusable buffer for overlap sphere

    private GameObject player;
    [SerializeField] private GameObject groundIndicatorPrefab;

    public override void OnEnable()
    {
        player = FindAnyObjectByType<CharacterController>().gameObject;
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
        yield return new WaitForSeconds(duration);
        ObjectPooler.Instance.ReturnObject(gameObject);
    }
    private Vector3 GetRandomPointInRange()
    {
        // Generate a random point within a circle araound the player
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        Vector3 targetPoint = new Vector3(randomPoint.x, 1, randomPoint.y) + player.transform.position;
        targetPoint.y = 1; // Ensure it's on the ground (Y = 1)
        return targetPoint;

    }

    public void Update()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0)
        {
            DamageNearbyEnemies();
            tickTimer = tickInterval;
        }

        SetIndicatorScale();
    }

    private void SetIndicatorScale()
    {
        if (groundIndicatorPrefab != null)
        {
            Vector3 scale = groundIndicatorPrefab.transform.localScale;
            scale.x = radius * 2;
            scale.y = radius * 2;
            scale.z = .5f;
            groundIndicatorPrefab.transform.localScale = scale;
        }
    }

    void DamageNearbyEnemies()
    {
        int hits = Physics.OverlapSphereNonAlloc(transform.position, radius /2, _hitBuffer);
        Collider[] colliders = new Collider[hits];
        for (int i = 0; i < hits; i++)
        {
            colliders[i] = _hitBuffer[i];
        }

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // Apply damage
                enemy.TakeDamage(damage);
            }
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
