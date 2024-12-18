using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public WeaponDataSO weaponData;
    private GameObject bulletPrefab;
    public Transform gun;
    public Transform bulletSpawn;
    public float fireRate = 0.5f;
    public float nextFire = 0.0f;
    public int damage;
    public float bulletSpeed;
    public float bulletLifeTime;

    public bool isWeponDataChanged = true;

    //Auto targeting and auto shooting

    public bool isAutoTargeting = false;
    public bool isAutoShooting = false;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bulletPrefab = weaponData.bulletPrefab;
        fireRate = weaponData.fireRate;
        damage = weaponData.damage;
        bulletSpeed = weaponData.bulletSpeed;
        bulletLifeTime = weaponData.bulletLifeTime;
    }

    private void Update()
    {
        if (isAutoShooting)
        {
            AutoShoot();
        }
        else
        {
            ManualShoot();
        }

        if (isAutoTargeting)
        {
            CharaterMovement.Instance.autoTargeting = true;
            AutoTarget();
        }
        else
        {
            CharaterMovement.Instance.autoTargeting = false;
        }
    }

    private void AutoShoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Fire();
        }
    }

    private void ManualShoot()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Fire();
        }
    }

    private void AutoTarget()
    {
        Vector3 targetDirection = GetTargetDirection();
        targetDirection.y = 0;

        Vector3 direction = targetDirection - gun.position;
        direction.y = 0;

        gun.transform.forward = direction;
    }

    private Vector3 GetTargetDirection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100f);
        List<Collider> enemies = new List<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemies enemy) && enemy.gameObject.activeSelf)
            {
                enemies.Add(collider);
            }
        }

        if (enemies.Count == 0)
        {
            return transform.position + transform.forward;
        }

        Collider closestEnemy = enemies.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
        return closestEnemy.transform.position;
    }

    private void Fire()
    {
        GameObject bullet = ObjectPooler.Instance.SpawnFromPool(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        if(isWeponDataChanged)
        {
            Initialize();
            isWeponDataChanged = false;
        }
                
        bullet.TryGetComponent(out Bullet bulletScript);
        bulletScript.Initialize(damage, bulletSpeed, bulletLifeTime);

    }
}
