using UnityEngine;

//this will be a pool of upgrades, when player level up x amount of upgrades will be shown to the player to choose from.

public class LevelUpPool : MonoBehaviour
{
    public Upgrade[] upgrades;

    public Upgrade GetRandomUpgrade()
    {
        return upgrades[Random.Range(0, upgrades.Length)];
    }

    public Upgrade GetUpgradeByID(int id)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i].upgradeID == id)
            {
                return upgrades[i];
            }
        }
        return null;
    }

    public Upgrade GetUpgradeByName(string name)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i].upgradeName == name)
            {
                return upgrades[i];
            }
        }
        return null;
    }
    public UpgradeRarity GiveARandomRarity()
    {
        int randomRarity = Random.Range(0, 100);
        if (randomRarity < 50)
        {
            return UpgradeRarity.Common;
        }
        else if (randomRarity < 80)
        {
            return UpgradeRarity.Rare;
        }
        else if (randomRarity < 95)
        {
            return UpgradeRarity.Epic;
        }
        else
        {
            return UpgradeRarity.Legendary;
        }
    }
}

[System.Serializable]
public class Upgrade
{
    public bool isWeaponUpgrade;
    public string upgradeName;
    public string upgradeDescription;
    public int upgradeID;
    public bool isPassive;
    public Sprite upgradeSprite;

    public UpgradeRarity rarity;
}



public enum UpgradeRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}