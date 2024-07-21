using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;
    public PlayerStats playerStats;
    public PlayerWeapon weaponData;
    public XPBar xpBar;

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

    private void Start()
    {
        playerStats = new PlayerStats(100, 0, CharaterMovement.Instance.currentSpeed);
    }

    //we will add experience to the player until the experience is NOT enough to level up, then we will set xpBar and other things.

    public void AddExperience(int xp)
    {
        experience += xp;
        xpBar.AddXP(experience);
        if(StillNeedToLevelUp())
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = (int)(experienceToNextLevel * 1.1f);
        
        GameManager.Instance.LevelUp();

        xpBar.SetLevel(level);
        xpBar.SetMaxXP(experienceToNextLevel);
        xpBar.ResetXP();
    }

    public bool StillNeedToLevelUp()
    {
        return experience >= experienceToNextLevel;
    }

    public void TakeDamage(int damage)
    {
        playerStats.health -= damage;
        if (playerStats.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player died!");
        GameManager.Instance.GameOver();
    }

    public void Heal(int amount)
    {
        playerStats.health += amount;
    }

    public void IncreaseArmor(int amount)
    {
        playerStats.armor += amount;
    }

    public void IncreaseSpeed(int amount)
    {
        playerStats.speed += amount;
        CharaterMovement.Instance.SetSpeed(amount);
    }

    public void IncreaseDamage(int amount)
    {
        weaponData.damage += amount;
        weaponData.isWeponDataChanged = true;
    }

    public void IncreaseFireRate(float amount)
    {
        weaponData.isWeponDataChanged = true;
        if (weaponData.fireRate - amount <= 0.001f)
        {
            weaponData.fireRate = 0.001f;
            return;
        }
        weaponData.fireRate -= amount;
    }

    public void IncreaseBulletSpeed(float amount)
    {
        weaponData.bulletSpeed += amount;
        weaponData.isWeponDataChanged = true;
    }

    public int RarityMultiplier(UpgradeRarity rarity)
    {
        switch (rarity)
        {
            case UpgradeRarity.Common:
                return 1;
            case UpgradeRarity.Rare:
                return 2;
            case UpgradeRarity.Epic:
                return 3;
            case UpgradeRarity.Legendary:
                return 4;
            default:
                return 1;
        }
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade.upgradeID)
        {
            case 0:
                Heal(10 * RarityMultiplier(upgrade.rarity));
                break;
            case 1:
                IncreaseArmor(5 * RarityMultiplier(upgrade.rarity));
                break;
            case 2:
                IncreaseSpeed(1 * RarityMultiplier(upgrade.rarity));
                break;
            case 3:
                IncreaseDamage(1 * RarityMultiplier(upgrade.rarity));
                break;
            case 4:
                IncreaseFireRate(0.01f * RarityMultiplier(upgrade.rarity));
                break;
            case 5:
                IncreaseBulletSpeed(1 * RarityMultiplier(upgrade.rarity));
                break;
            }
        }
    }

[Serializable]
public class PlayerStats
{
    public int health;
    public int armor;
    public float speed;

    public PlayerStats(int health, int armor, float speed)
    {
        this.health = health;
        this.armor = armor;
        this.speed = speed;
    }
}
