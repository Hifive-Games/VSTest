using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public float detectionRange = 15f;
    public float shootCooldown = 1f;
    private float shootTimer = 0f;

    public GameObject bullet;

    public void Shoot()
    {
        ObjectPooler.Instance.SpawnFromPool(bullet, transform.position, transform.rotation);
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        Enemy closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
            if (distance <= detectionRange)
            {
                // Look at enemy
                transform.LookAt(closestEnemy.transform);

                // Shoot if ready
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

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }
}
