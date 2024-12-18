using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<IWeapon> EquippedWeapons = new List<IWeapon>();

    public void ApplyUpgrade(Upgrade upgrade)
    {
        if (upgrade.Type != UpgradeType.Weapon) return;

        foreach (IWeapon weapon in EquippedWeapons)
        {
            if (weapon.WeaponID == upgrade.TargetID || upgrade.TargetID == 0)
            {
                weapon.Upgrade(upgrade);
            }
        }
    }
    public void ResetWeapons()
    {
        foreach (var weapon in EquippedWeapons)
        {
            weapon.ResetStats();
        }
    }

    public void EquipWeapon(IWeapon weapon)
    {
        if (!EquippedWeapons.Contains(weapon))
        {
            EquippedWeapons.Add(weapon);
            // Initialize the weapon if necessary
        }
    }
}