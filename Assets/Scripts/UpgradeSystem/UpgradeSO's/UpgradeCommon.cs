using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCommon : ScriptableObject
{
    public string
    ID,
    Name,
    Description;

    public Upgradetype upgradetype;

    public List<Sprite> Icons;

    public bool isUnlocked;

    public List<UpgradeEffects> upgradeEffects;

    public Rarity rarity;

    public int currentLevel;
}

public enum Upgradetype
{
    Default,
    Global,
    Charcater,
    Weapon,
    Spell,
    Unique
}



public enum Rarity
{
    Common,
    unCommon,
    Rare,
    Epic,
    Legendary
}
