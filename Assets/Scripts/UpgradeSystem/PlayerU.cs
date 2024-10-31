using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerU
{
    public static PlayerU Instance ;
    public List<string> Upgrades { get; private set; }
    public Character Character { get; private set; }
    public Weapon Weapon { get; private set; }

    public PlayerU()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Upgrades = new List<string>();
    }

    public void AddUpgrade(UpgradeSO upgrade)
    {
        Upgrades.Add(upgrade.upgradeName);
        upgrade.ApplyEffect(this);
    }

    public void SetCharacter(Character character)
    {
        Character = character;
    }

    public void SetWeapon(Weapon weapon)
    {
        Weapon = weapon;
    }
}

public class Weapon
{
    public string name;
    public List<string> availableUpgrades;

    public bool CanApplyUpgrade(UpgradeSO upgrade)
    {
        return availableUpgrades.Contains(upgrade.upgradeName);
    }
}

public class Character
{
    public string name;
    public List<string> availableUpgrades;

    public bool CanApplyUpgrade(UpgradeSO upgrade)
    {
        return availableUpgrades.Contains(upgrade.upgradeName);
    }
}
