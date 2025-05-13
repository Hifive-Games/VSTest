using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DroneWeapon : MonoBehaviour
{
    public GameObject bullet;
    [SerializeField] private Transform shooterParent;

    public float detectionRange = 15f;
    public float attackSpeed = 1f; // saniyede kaç atış yapılır
    public float attackSize = 1f;
    private float shootTimer = 0f;

    private float searchInterval = 0.2f;
    private float searchTimer = 0f;

    private Enemy currentTarget;

  
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
                    shootTimer = 1f / Mathf.Max(attackSpeed, 0.1f); // 0'a bölünme hatasını önler
                }
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = ObjectPooler.Instance.SpawnFromPool(bullet.gameObject, shooterParent.position, transform.rotation);
        bulletObj.transform.localScale = Vector3.one * attackSize;
        SFXManager.Instance.PlayAt(SFX.BulletFire); // Play bullet fire sound effect
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
