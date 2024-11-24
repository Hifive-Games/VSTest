using UnityEngine;

public class UpgradeSystemTest : MonoBehaviour
{
    public UpgradeHandler upgradeHandler;
    public WeaponUpgradeSO testWeapon; // Assign in the inspector
    public SpellUpgradeSO testSpell; // Assign in the inspector
    public int numberOfRandomUpgrades = 3; // Number of upgrades to show on level up
    public bool testLevelUp; // Check to simulate level up
    public bool testLevelUpWithSpecificPool; // Check to simulate level up for specific pool
    public UpgradePoolType type;
    public bool showAllUpgrades; // Check to get all upgrades
    public bool applyGlobalUpgrades; // Check to apply global upgrades


    private void Start()
    {
        // Populate the Weapon, Spell, and Global pools
        PopulateAllUpgradePools();
    }

    // This method will populate all upgrade pools based on available weapons, spells, and globals
    private void PopulateAllUpgradePools()
    {
        // If a weapon is available, populate its corresponding pool
        if (testWeapon != null)
        {
            foreach (var upgrade in testWeapon.upgrades)
            {
                upgradeHandler.AddUpgradeToPool(upgrade, UpgradePoolType.Weapon);
            }
        }

        // If a spell is available, populate its corresponding pool
        if (testSpell != null)
        {
            foreach (var upgrade in testSpell.upgrades)
            {
                upgradeHandler.AddUpgradeToPool(upgrade, UpgradePoolType.Spell);
            }
        }

        // Add global upgrades from resources (this step is done in UpgradeHandler now)
        upgradeHandler.LoadGlobalUpgrades();  // This is handled in the UpgradeHandler itself
    }

    private void Test()
    {
        if (testLevelUp)
        {
            // Simulate level up and display random upgrades
            Debug.Log("Level Up! Selecting Random Upgrades...");
            upgradeHandler.SelectRandomUpgradesOnLevelUp(numberOfRandomUpgrades);
            testLevelUp = false;
        }

        if (testLevelUpWithSpecificPool)
        {
            // Simulate level up and display random upgrades
            Debug.Log("Level Up! Selecting Random Upgrades...");
            upgradeHandler.SelectRandomUpgradesOnLevelUp(numberOfRandomUpgrades,type);
            testLevelUpWithSpecificPool = false;
        }

        if (showAllUpgrades)
        {
            // Check if the pools have been populated correctly
            upgradeHandler.PrintAllUpgradePools();
            showAllUpgrades = false;
        }

        if (applyGlobalUpgrades)
        {
            // Apply global upgrades
            GlobalUpgradeSO upgrade = upgradeHandler.SelectRandomUpgrade();

            bool success = upgradeHandler.ApplyUpgrade(upgrade);

            // Log the result
            if (success)
            {
                Debug.Log($"Upgrade {upgrade.Name} applied successfully.");
            }
            else
            {
                Debug.LogWarning($"Upgrade {upgrade.Name} could not be applied.");
            }

            applyGlobalUpgrades = false;
        }
    }

    private void Update()
    {
        // Test methods are called once per frame
        Test();
    }
}
