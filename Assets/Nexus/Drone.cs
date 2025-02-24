using UnityEngine;

public class Drone : MonoBehaviour
{
    public float detectionRange = 15f;
    public float shootCooldown = 1f;
    private float shootTimer = 0f;
    public GameObject bullet;

    private float searchInterval = 0.2f;
    private float searchTimer = 0f;
    private Enemy currentTarget;

    public void Shoot()
    {
        ObjectPooler.Instance.SpawnFromPool(bullet, transform.position, transform.rotation);
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        searchTimer -= Time.deltaTime;
        if (searchTimer <= 0f)
        {
            currentTarget = FindClosestEnemy();
            searchTimer = searchInterval;
        }

        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (distance <= detectionRange)
            {
                transform.LookAt(currentTarget.transform);

                if (shootTimer <= 0f)
                {
                    Shoot();
                    shootTimer = shootCooldown;
                }
            }
        }
    }

    private Enemy FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enemy nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(currentPos, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }
}