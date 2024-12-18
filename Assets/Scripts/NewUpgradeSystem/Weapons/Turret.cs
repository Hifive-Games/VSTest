using UnityEngine;
using System.Collections.Generic;
public class Turret : IWeapon
{
    public int WeaponID { get; private set; }
    public string WeaponName { get; private set; }
    public string WeaponDescription { get; private set; }
    public int Damage { get; set; }
    public float FireRate { get; set; }
    public float ReloadTime { get; set; }
    public float BulletSpeed { get; set; }
    public float BulletLifeTime { get; set; }
    public int WeaponCount { get; set; }
    public int BaseDamage { get; private set; }
    public float BaseFireRate { get; private set; }
    public List<Upgrade> appliedUpgrades { get; }

    private GameObject bulletPrefab;
    private GameObject weaponPrefab;
    private Transform firePoint;

    public Turret()
    {
        BaseDamage = 10;
        BaseFireRate = 0.5f;
        appliedUpgrades = new List<Upgrade>();

        WeaponID = 1;
        WeaponName = "Turret";
        WeaponDescription = "A stationary turret that fires at enemies.";
        Damage = BaseDamage;
        FireRate = BaseFireRate;
        ReloadTime = 2f;
        BulletSpeed = 10f;
        BulletLifeTime = 2f;
        WeaponCount = 1;
    }

    public void Initialize(GameObject bulletPrefab, GameObject weaponPrefab)
    {
        this.bulletPrefab = bulletPrefab;
        this.weaponPrefab = weaponPrefab;

        // Instantiate the turret in the game world
        GameObject turretInstance = Object.Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity);
        firePoint = turretInstance.transform.Find("FirePoint"); // Ensure there's a child named FirePoint
    }

    public void Equip()
    {
        // Logic for equipping the turret
        Debug.Log($"{WeaponName} equipped.");
    }

    public void Shoot()
    {
        for (int i = 0; i < WeaponCount; i++)
        {
            GameObject bullet = Object.Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(Damage, BulletSpeed, BulletLifeTime);
        }
    }

    public void Reload()
    {
        // Logic for reloading if necessary
        Debug.Log($"{WeaponName} is reloading.");
    }

    public void Upgrade(Upgrade upgrade)
    {
        if (upgrade.Type != UpgradeType.Weapon) return;

        switch (upgrade.Target)
        {
            case UpgradeTarget.WeaponDamage:
                Damage += (int)upgrade.Value;
                break;
            case UpgradeTarget.WeaponFireRate:
                FireRate = Mathf.Max(0.1f, FireRate + upgrade.Value);
                break;
            case UpgradeTarget.WeaponReloadTime:
                ReloadTime = Mathf.Max(0.1f, ReloadTime - upgrade.Value);
                break;
            case UpgradeTarget.WeaponBulletSpeed:
                BulletSpeed += upgrade.Value;
                break;
            case UpgradeTarget.WeaponBulletLifeTime:
                BulletLifeTime += upgrade.Value;
                break;
            case UpgradeTarget.WeaponCount:
                WeaponCount += (int)upgrade.Value;
                break;
            default:
                break;
        }

        appliedUpgrades.Add(upgrade);
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        // Handle upgrades
        if (upgrade.Type != UpgradeType.Weapon) return;

        switch (upgrade.Target)
        {
            case UpgradeTarget.WeaponDamage:
                Damage += (int)upgrade.Value;
                break;
            case UpgradeTarget.WeaponFireRate:
                FireRate = Mathf.Max(0.1f, FireRate - upgrade.Value);
                break;
            case UpgradeTarget.WeaponReloadTime:
                ReloadTime = Mathf.Max(0.1f, ReloadTime - upgrade.Value);
                break;
            case UpgradeTarget.WeaponBulletSpeed:
                BulletSpeed += upgrade.Value;
                break;
            case UpgradeTarget.WeaponBulletLifeTime:
                BulletLifeTime += upgrade.Value;
                break;
            case UpgradeTarget.WeaponCount:
                WeaponCount += (int)upgrade.Value;
                break;
        }

        appliedUpgrades.Add(upgrade);
    }

    public List<Upgrade> GetAppliedUpgrades()
    {
        return new List<Upgrade>(appliedUpgrades);
    }

    public void ResetStats()
    {
        Damage = BaseDamage;
        appliedUpgrades.Clear();
    }
}