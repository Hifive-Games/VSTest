using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    public Sprite icon;
    public string upgradeName;
    public string description;
    public List<string> prerequisites; // Upgrades that need to be applied before this one
    public List<string> incompatibleUpgrades; // Upgrades this cannot coexist with
    public UpgradeType type;
    public Action<PlayerU> ApplyEffect;
}

public enum UpgradeType
{
    Global,
    WeaponSpecific,
    CharacterSpecific
}
