using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public static Player Instance;

    public int BaseHealth { get; private set; }
    public int Health { get; set; }
    private List<Upgrade> appliedUpgrades;

    public List<IWeapon> EquippedWeapons { get; private set; }
    public List<ISpell> EquippedSpells { get; private set; }

    public int ExperienceToNextLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            BaseHealth = 100;
            Health = BaseHealth;
            appliedUpgrades = new List<Upgrade>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        if (upgrade.Type != UpgradeType.Player) return;

        switch (upgrade.Target)
        {
            case UpgradeTarget.PlayerHealth:
                Health += (int)upgrade.Value;
                break;
            // Handle other targets if needed
        }

        appliedUpgrades.Add(upgrade);
    }

    public List<Upgrade> GetAppliedUpgrades()
    {
        return new List<Upgrade>(appliedUpgrades);
    }

    public void ResetPlayerStats()
    {
        Health = BaseHealth;
        appliedUpgrades.Clear();
    }

    public void AddExperience(int experience)
    {
        // Add experience logic here
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            // Handle player death
        }
    }
}