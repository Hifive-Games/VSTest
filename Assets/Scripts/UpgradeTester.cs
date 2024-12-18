using UnityEngine;
using System.Collections.Generic;
using NEXUS.Utilities;
using System.Collections;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class UpgradeTester : MonoBehaviour
{
    private WeaponManager weaponManager;
    private SpellManager spellManager;
    private Player player;
    private JSONDataHandler jsonDataHandler;

    private void Start()
    {
        // Initialize managers
        weaponManager = WeaponManager.Instance;
        spellManager = SpellManager.Instance;
        player = Player.Instance;
        jsonDataHandler = new JSONDataHandler("TestSaveSlot");

        // Run the test
        StartCoroutine(RunUpgradeTest());
    }

    private IEnumerator RunUpgradeTest()
    {
        Debug.Log("Starting Upgrade Test...");

        // Equip a weapon and a spell
        IWeapon testWeapon = new Turret();
        weaponManager.EquipWeapon(testWeapon);

        ISpell testSpell = new FireballSpell();
        spellManager.EquipSpell(testSpell);

        yield return null; // Wait a frame to ensure everything is initialized

        // Reset stats
        ResetStats();

        // Load upgrades
        LoadUpgrades();

        // Verify upgrades
        VerifyUpgrades(testWeapon, testSpell);

        // Save upgrades
        SaveUpgrades();

        Debug.Log("Upgrade Test Completed.");
    }

    private void SaveUpgrades()
    {
        UpgradeData upgradeData = new UpgradeData();
        upgradeData.Upgrades = GetAllAppliedUpgrades();
        jsonDataHandler.SaveData(upgradeData, "Upgrades.json");
        Debug.Log("Upgrades saved.");
    }

    private void LoadUpgrades()
    {
        UpgradeData loadedData = jsonDataHandler.LoadData<UpgradeData>("Upgrades.json");
        if (loadedData != null)
        {
            foreach (var upgrade in loadedData.Upgrades)
            {
                switch (upgrade.Type)
                {
                    case UpgradeType.Weapon:
                        weaponManager.ApplyUpgrade(upgrade);
                        break;
                    case UpgradeType.Player:
                        player.ApplyUpgrade(upgrade);
                        break;
                    case UpgradeType.Spell:
                        spellManager.ApplyUpgrade(upgrade);
                        break;
                }
                
            }
            Debug.Log("Upgrades loaded and applied.");
        }
        else
        {
            Debug.LogError("Failed to load upgrades.");
        }
    }

    private void ResetStats()
    {
        weaponManager.ResetWeapons();
        player.ResetPlayerStats();
        spellManager.ResetSpells();
        Debug.Log("Stats reset.");
    }

    private void VerifyUpgrades(IWeapon weapon, ISpell spell)
    {
        float expectedSpellCooldown = (spell as FireballSpell).BaseCooldown - 0.5f;
        if (expectedSpellCooldown == spell.Cooldown)
        {
            Debug.Log($"Spell cooldown verified: {spell.Cooldown}");
        }
        else
        {
            Debug.LogError($"{spell.SpellName} cooldown mismatch: Expected {expectedSpellCooldown}, Got {spell.Cooldown}");
        }

        float expectedFireRate = (weapon as Turret).BaseFireRate - 0.1f;
        if (expectedFireRate == weapon.FireRate)
        {
            Debug.Log($"{weapon.WeaponName} fire rate verified: {weapon.FireRate}");
        }
        else
        {
            Debug.LogError($"Weapon fire rate mismatch: Expected {expectedFireRate}, Got {weapon.FireRate}");
        }
    }

    private List<Upgrade> GetAllAppliedUpgrades()
    {
        var upgrades = new List<Upgrade>();

        // Get weapon upgrades
        foreach (var weapon in weaponManager.EquippedWeapons)
        {
            upgrades.AddRange(weapon.GetAppliedUpgrades());
        }

        // Get player upgrades
        upgrades.AddRange(player.GetAppliedUpgrades());

        // Get spell upgrades
        foreach (var spell in spellManager.EquippedSpells)
        {
            upgrades.AddRange(spell.GetAppliedUpgrades());
        }

        return upgrades;
    }
}