using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.Instance.AddExperience(100);
        }
    }
}

public class UpgradePointer : MonoBehaviour
{
    public Upgrade upgrade;

    public void ApplyUpgrade()
    {
        switch (upgrade.Type)
        {
            case UpgradeType.Player:
                Player.Instance.ApplyUpgrade(upgrade);
                break;
            case UpgradeType.Weapon:
                foreach (IWeapon weapon in Player.Instance.EquippedWeapons)
                {
                    if (weapon.WeaponID == upgrade.TargetID || upgrade.TargetID == 0)
                    {
                        weapon.Upgrade(upgrade);
                    }
                }
                break;
            case UpgradeType.Spell:
                SpellManager.Instance.ApplyUpgrade(upgrade);
                break;
        }
    }
}
