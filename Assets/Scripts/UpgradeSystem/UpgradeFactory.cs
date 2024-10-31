using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFactory
{
    private List<UpgradeSO> allUpgrades;

    public UpgradeFactory(List<UpgradeSO> allUpgrades)
    {
        this.allUpgrades = allUpgrades;
    }

    public List<UpgradeSO> GetAvailableUpgrades(PlayerU player)
    {
        List<UpgradeSO> availableUpgrades = new List<UpgradeSO>();
        
        foreach (UpgradeSO upgrade in allUpgrades)
        {
            bool prerequisitesMet = upgrade.prerequisites.TrueForAll(prerequisite => player.Upgrades.Contains(prerequisite));
            bool noIncompatibilities = upgrade.incompatibleUpgrades.TrueForAll(incompatibility => !player.Upgrades.Contains(incompatibility));
            
            if (prerequisitesMet && noIncompatibilities && IsUpgradeAllowed(player, upgrade))
            {
                availableUpgrades.Add(upgrade);
            }
            else
                Debug.Log("Not meet prerequest.");
        }
        
        return availableUpgrades;
    }

    private bool IsUpgradeAllowed(PlayerU player, UpgradeSO upgrade)
    {
        // Implement character-specific and weapon-specific logic here
        switch (upgrade.type)
        {
            case UpgradeType.Global:
                return true;
            case UpgradeType.WeaponSpecific:
                return player.Weapon.CanApplyUpgrade(upgrade);
            case UpgradeType.CharacterSpecific:
                return player.Character.CanApplyUpgrade(upgrade);
            default:
                return false;
        }
    }
}

