using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DroneWeapon : MonoBehaviour
{
    public float detectionRange = 15f;
    public float shootCooldown = 1f;
    private float shootTimer = 0f;
    public GameObject bullet;

    private float searchInterval = 0.2f;
    private float searchTimer = 0f;
    private TestEnemyDed currentTarget;

    public Transform shooterParent;
    
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

<<<<<<< Updated upstream
    private TestEnemyDed FindClosestEnemy()
=======
    public void Shoot()
    {
        Instantiate(bullet, shooterParent.position, transform.rotation);
    }
    
    private Enemy FindClosestEnemy()
>>>>>>> Stashed changes
    {
        TestEnemyDed[] enemies = FindObjectsOfType<TestEnemyDed>();
        TestEnemyDed nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (TestEnemyDed enemy in enemies)
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
