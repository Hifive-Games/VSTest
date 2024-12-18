using System.Collections.Generic;

[System.Serializable]
public class Upgrade
{
    public int TargetID;
    public string Name;
    public string Description;
    public UpgradeType Type; // Weapon, Player, Spell
    public UpgradeTarget Target;
    public float Value;
    public int maxUpgrades = 5;
    public List<int> LevelValues = new List<int>();

    public Upgrade(string name,string description, int targetID, UpgradeType type, UpgradeTarget target, float value)
    {
        Name = name;
        Description = description;
        TargetID = targetID;
        Type = type;
        Target = target;
        Value = value;
    }
}

// Enums need to be marked as serializable if they're nested within a class
[System.Serializable]
public enum UpgradeType
{
    Weapon,
    Player,
    Spell
}

[System.Serializable]
public enum UpgradeTarget
{
    // Weapon Upgrades
    WeaponDamage,
    WeaponFireRate,
    WeaponReloadTime,
    WeaponBulletSpeed,
    WeaponBulletLifeTime,
    WeaponCount,

    // Player Upgrades
    PlayerHealth,
    PlayerSpeed,

    // Spell Upgrades
    SpellDamage,
    SpellCooldown,
    SpellRange,
    SpellDuration
}