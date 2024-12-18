using UnityEngine;
using System.Collections.Generic;

public class FireballSpell : ISpell
{
    public int SpellID { get; private set; }
    public string SpellName { get; private set; }
    public string SpellDescription { get; private set; }
    public int Damage { get; set; }
    public float Cooldown { get; set; }
    public float Range { get; set; }
    public float Duration { get; set; }
    public int BaseDamage { get; private set; }
    public float BaseCooldown { get; private set; }
    public List<Upgrade> appliedUpgrades { get;}

    public FireballSpell()
    {
        BaseDamage = 30;
        SpellID = 1;
        BaseCooldown = 5f;
        appliedUpgrades = new List<Upgrade>();
        SpellName = "Fireball";
        SpellDescription = "Launches a fiery ball that damages enemies.";
        Damage = BaseDamage;
        Cooldown = BaseCooldown;
        Range = 10f;
        Duration = 2f;
    }

    public void Cast()
    {
        // Logic for casting the fireball spell
        Debug.Log($"{SpellName} casted.");
    }

    public void Upgrade(Upgrade upgrade)
    {
        if (upgrade.Type != UpgradeType.Spell) return;

        switch (upgrade.Target)
        {
            case UpgradeTarget.SpellDamage:
                Damage += (int)upgrade.Value;
                break;
            case UpgradeTarget.SpellCooldown:
                Cooldown = Mathf.Max(0.1f, Cooldown + upgrade.Value);
                break;
            case UpgradeTarget.SpellRange:
                Range += upgrade.Value;
                break;
            case UpgradeTarget.SpellDuration:
                Duration += upgrade.Value;
                break;
            default:
                break;
        }

         appliedUpgrades.Add(upgrade);
    }
    public void ApplyUpgrade(Upgrade upgrade)
    {
        // Handle upgrades
        if (upgrade.Type != UpgradeType.Spell) return;

        switch (upgrade.Target)
        {
            case UpgradeTarget.SpellDamage:
                Damage += (int)upgrade.Value;
                break;
            case UpgradeTarget.SpellCooldown:
                Cooldown = Mathf.Max(0.1f, Cooldown - upgrade.Value);
                break;
            case UpgradeTarget.SpellRange:
                Range += upgrade.Value;
                break;
            case UpgradeTarget.SpellDuration:
                Duration += upgrade.Value;
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