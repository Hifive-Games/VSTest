using UnityEngine;
using System.Collections.Generic;
public interface IWeapon
{
    int WeaponID { get; }
    string WeaponName { get; }
    string WeaponDescription { get; }
    int Damage { get; set; }
    float FireRate { get; set; }
    float ReloadTime { get; set; }
    float BulletSpeed { get; set; }
    float BulletLifeTime { get; set; }
    int WeaponCount { get; set; }
    List<Upgrade> appliedUpgrades { get; }

    int BaseDamage { get; }
    void Initialize(GameObject bulletPrefab, GameObject weaponPrefab);
    void Equip();
    void Shoot();
    void Reload();
    void Upgrade(Upgrade upgrade);
    void ResetStats();
    List<Upgrade> GetAppliedUpgrades();
}