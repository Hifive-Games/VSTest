using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeSystem
{
    private List<UpgradeSO> globalUpgrades;
    private Dictionary<string, List<UpgradeSO>> characterSpecificUpgrades;
    private Dictionary<string, List<UpgradeSO>> weaponSpecificUpgrades;

    public UpgradeSystem(List<UpgradeSO> globalUpgrades,
                         Dictionary<string, List<UpgradeSO>> characterSpecificUpgrades,
                         Dictionary<string, List<UpgradeSO>> weaponSpecificUpgrades)
    {
        this.globalUpgrades = globalUpgrades;
        this.characterSpecificUpgrades = characterSpecificUpgrades;
        this.weaponSpecificUpgrades = weaponSpecificUpgrades;
    }

    public List<UpgradeSO> GetAvailableUpgrades(PlayerU player)
    {
        List<UpgradeSO> availableUpgrades = new List<UpgradeSO>(globalUpgrades);
        if (characterSpecificUpgrades.TryGetValue(player.Character.name, out var charUpgrades))
        {
            availableUpgrades.AddRange(charUpgrades);
        }
        if (weaponSpecificUpgrades.TryGetValue(player.Weapon.name, out var weaponUpgrades))
        {
            availableUpgrades.AddRange(weaponUpgrades);
        }
        return availableUpgrades.Where(upgrade =>
                upgrade.prerequisites.All(prerequisite => player.Upgrades.Contains(prerequisite))
                && upgrade.incompatibleUpgrades.All(incompatible => !player.Upgrades.Contains(incompatible)))
                .ToList();
    }
}

