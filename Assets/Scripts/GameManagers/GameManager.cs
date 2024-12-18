using UnityEngine;
using System.Collections.Generic;
using NEXUS.Utilities;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;

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
        jsonDataHandler = new JSONDataHandler("SaveSlot1");

        // Load and apply upgrades
        LoadAndApplyUpgrades();
    }

    private void LoadAndApplyUpgrades()
    {
        UpgradeData loadedUpgradeData = jsonDataHandler.LoadData<UpgradeData>("Upgrades.json");
        if (loadedUpgradeData != null)
        {
            foreach (Upgrade upgrade in loadedUpgradeData.Upgrades)
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
        }
        else
        {
            Debug.Log("No upgrade data found. Starting with default values.");
        }
    }

    public void SaveUpgrades()
    {
        // Collect upgrades from your game state or inventories
        UpgradeData upgradeData = new UpgradeData();
        // Assume you have a way to get all currently applied upgrades
        upgradeData.Upgrades = GetAllCurrentUpgrades();

        // Save upgrades
        jsonDataHandler.SaveData(upgradeData, "Upgrades.json");
    }

    private List<Upgrade> GetAllCurrentUpgrades()
    {
        List<Upgrade> upgrades = new List<Upgrade>();

        // Collect weapon upgrades
        foreach (IWeapon weapon in weaponManager.EquippedWeapons)
        {
            // Assuming each weapon keeps track of its applied upgrades
            upgrades.AddRange(weapon.GetAppliedUpgrades());
        }

        // Collect player upgrades
        upgrades.AddRange(player.GetAppliedUpgrades());

        // Collect spell upgrades
        foreach (ISpell spell in spellManager.EquippedSpells)
        {
            upgrades.AddRange(spell.GetAppliedUpgrades());
        }

        return upgrades;
    }
}