using System.Collections.Generic;
using UnityEngine;

public enum UpgradePoolType
{
    Global,
    Weapon,
    Spell,
    Unique
}

public class UpgradeHandler : MonoBehaviour
{
    private Dictionary<UpgradePoolType, List<GlobalUpgradeSO>> upgradePools;

    public void Awake()
    {
        InitializePools();
    }

    void OnDestroy()
    {
        ResetSOValues();
    }

    private void InitializePools()
    {
        upgradePools = new Dictionary<UpgradePoolType, List<GlobalUpgradeSO>>();
        foreach (UpgradePoolType type in System.Enum.GetValues(typeof(UpgradePoolType)))
        {
            upgradePools[type] = new List<GlobalUpgradeSO>();
        }
    }

    public void RefreshAllPools()
    {
        foreach (UpgradePoolType poolType in System.Enum.GetValues(typeof(UpgradePoolType)))
        {
            upgradePools[poolType].Clear();
        }

        PopulateAllUpgradesPool();
    }

    public void ResetSOValues()
    {
        foreach (UpgradePoolType poolType in System.Enum.GetValues(typeof(UpgradePoolType)))
        {
            List<GlobalUpgradeSO> upgrades = GetUpgradesByType(poolType);
            foreach (GlobalUpgradeSO upgrade in upgrades)
            {
                upgrade.currentLevel = 0;
                upgrade.isUnlocked = false;
                upgrade.rarity = Rarity.Common;
            }
        }
    }

    public void LoadGlobalUpgrades()
    {
        GlobalUpgradeSO[] globalUpgrades = Resources.LoadAll<GlobalUpgradeSO>("Upgrades/Global");
        foreach (var upgrade in globalUpgrades)
        {
            AddUpgradeToPool(upgrade, UpgradePoolType.Global);
        }
        Debug.Log($"{globalUpgrades.Length} Global upgrades loaded.");
    }

    public void AddUpgradeToPool(GlobalUpgradeSO upgrade, UpgradePoolType poolType)
    {
        if (upgradePools.ContainsKey(poolType))
        {
            upgradePools[poolType].Add(upgrade);
        }
        else
        {
            Debug.LogWarning($"Pool type {poolType} not found.");
        }
    }

    public void RemoveUpgradeFromPool(GlobalUpgradeSO upgrade, UpgradePoolType poolType)
    {
        if (upgradePools.ContainsKey(poolType))
        {
            upgradePools[poolType].Remove(upgrade);
        }
        else
        {
            Debug.LogWarning($"Pool type {poolType} not found.");
        }
    }

    public List<GlobalUpgradeSO> GetUpgradesByType(UpgradePoolType poolType)
    {
        if (upgradePools.ContainsKey(poolType))
        {
            return new List<GlobalUpgradeSO>(upgradePools[poolType]);
        }
        else
        {
            Debug.LogWarning($"Pool type {poolType} not found.");
            return new List<GlobalUpgradeSO>();
        }
    }

    public void PopulateAllUpgradesPool(WeaponUpgradeSO weapon = null, SpellUpgradeSO spell = null)
    {
        if (weapon != null)
        {
            foreach (GlobalUpgradeSO upgrade in weapon.upgrades)
            {
                if (upgrade.currentLevel < upgrade.maximumLevel)
                {
                    AddUpgradeToPool(upgrade, UpgradePoolType.Weapon);
                }
            }
        }

        if (spell != null)
        {
            foreach (GlobalUpgradeSO upgrade in spell.upgrades)
            {
                if (upgrade.currentLevel < upgrade.maximumLevel)
                {
                    AddUpgradeToPool(upgrade, UpgradePoolType.Spell);
                }
            }
        }

        foreach (UpgradePoolType poolType in System.Enum.GetValues(typeof(UpgradePoolType)))
        {
            List<GlobalUpgradeSO> availableUpgrades = GetUpgradesByType(poolType);
            foreach (GlobalUpgradeSO upgrade in availableUpgrades)
            {
                if (upgrade.currentLevel < upgrade.maximumLevel &&
                    !upgrade.preRequesties.Exists(preReq => !preReq.isUnlocked))
                {
                    AddUpgradeToPool(upgrade, poolType);
                }
            }
        }
    }

    public void SelectRandomUpgradesOnLevelUp(int count, UpgradePoolType upgradePoolType = UpgradePoolType.Global)
    {
        List<GlobalUpgradeSO> availableUpgrades = GetUpgradesByType(upgradePoolType)
            .FindAll(upgrade => upgrade.currentLevel < upgrade.maximumLevel);

        availableUpgrades = ShuffleList(availableUpgrades);
        Debug.Log($"Leveling up! Choices available from {upgradePoolType} pool:");

        int choicesCount = Mathf.Min(count, availableUpgrades.Count);
        for (int i = 0; i < choicesCount; i++)
        {
            GlobalUpgradeSO selectedUpgrade = availableUpgrades[i];
            Rarity randomRarity = GetRandomRarityForUpgrade(selectedUpgrade);

            Debug.Log($"Choice {i + 1}: {selectedUpgrade.Name} (Rarity: {randomRarity})\n" +
                      $"Description: {selectedUpgrade.Description}");
        }
    }

    public GlobalUpgradeSO SelectRandomUpgrade(UpgradePoolType upgradePoolType = UpgradePoolType.Global)
    {
        List<GlobalUpgradeSO> availableUpgrades = GetUpgradesByType(upgradePoolType);
        availableUpgrades = ShuffleList(availableUpgrades);

        GlobalUpgradeSO selectedUpgrade = availableUpgrades[0];
        Rarity randomRarity = GetRandomRarityForUpgrade(selectedUpgrade);

        selectedUpgrade.rarity = randomRarity;

        Debug.Log($"Selected Upgrade: {selectedUpgrade.Name} (Rarity: {randomRarity})\n" +
                  $"Description: {selectedUpgrade.Description}");

        return selectedUpgrade;
    }

    public bool ApplyUpgrade(GlobalUpgradeSO upgrade)
    {
        if (upgrade.preRequesties.Exists(preReq => !preReq.isUnlocked))
        {
            Debug.LogWarning($"Upgrade {upgrade.Name} cannot be applied due to unmet prerequisites.");
            return false;
        }

        if (upgrade.maximumLevel > 0 && upgrade.currentLevel >= upgrade.maximumLevel)
        {
            Debug.LogWarning($"Upgrade {upgrade.Name} is already at max level.");
            RemoveUpgradeFromPool(upgrade, UpgradePoolType.Global);
            return false;
        }

        if (upgrade.incompatibleUpgrades.Exists(incompatible => incompatible.isUnlocked))
        {
            Debug.LogWarning($"Upgrade {upgrade.Name} is incompatible with another unlocked upgrade.");
            return false;
        }

        float value = GetUpgradeValue(upgrade);
        foreach (var effect in upgrade.upgradeEffects)
        {
            effect.ApplyUpgrade(value);
        }

        upgrade.currentLevel++;
        Debug.Log($"Upgrade {upgrade.Name} applied. Current level: {upgrade.currentLevel}");

        if (upgrade.maximumLevel > 0 && upgrade.currentLevel >= upgrade.maximumLevel)
        {
            Debug.Log($"Upgrade {upgrade.Name} reached max level and will be removed from the pool.");
            RemoveUpgradeFromPool(upgrade, UpgradePoolType.Global);
        }

        return true;
    }

    public void RemoveUpgrade(GlobalUpgradeSO upgrade)
    {
        foreach (var effect in upgrade.upgradeEffects)
        {
            effect.RemoveUpgrade(upgrade.valueforEachLevel[upgrade.currentLevel]);
        }
    }

    private Rarity GetRandomRarityForUpgrade(GlobalUpgradeSO upgrade)
    {
        int totalWeight = 0;
        foreach (var rarityWeight in upgrade.rarityWeights)
        {
            totalWeight += rarityWeight.weight;
        }

        int randomValue = Random.Range(0, totalWeight);

        foreach (var rarityWeight in upgrade.rarityWeights)
        {
            randomValue -= rarityWeight.weight;
            if (randomValue < 0)
            {
                return rarityWeight.rarity;
            }
        }

        return Rarity.Common;
    }

    public int GetRarityIndex(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 0;
            case Rarity.unCommon: return 1;
            case Rarity.Rare: return 2;
            case Rarity.Epic: return 3;
            case Rarity.Legendary: return 4;
            default: return 0;
        }
    }

    public float GetUpgradeValue(GlobalUpgradeSO upgrade)
    {
        int rarityIndex = GetRarityIndex(upgrade.rarity);
        float value = upgrade.valueforEachLevel[upgrade.currentLevel];
        value += upgrade.valueforEachRarityLevel[rarityIndex];
        return value;
    }

    private List<GlobalUpgradeSO> ShuffleList(List<GlobalUpgradeSO> list)
    {
        List<GlobalUpgradeSO> shuffledList = new List<GlobalUpgradeSO>(list);
        for (int i = 0; i < shuffledList.Count; i++)
        {
            GlobalUpgradeSO temp = shuffledList[i];
            int randomIndex = Random.Range(i, shuffledList.Count);
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }
        return shuffledList;
    }

    internal void PrintAllUpgradePools()
    {
        foreach (var pool in upgradePools)
        {
            UpgradePoolType poolType = pool.Key;
            List<GlobalUpgradeSO> upgrades = pool.Value;

            Debug.Log($"--- {poolType} Pool ---");
            if (upgrades.Count > 0)
            {
                foreach (var upgrade in upgrades)
                {
                    Debug.Log($" ID: {upgrade.ID}, Upgrade: {upgrade.Name}, Level: {upgrade.currentLevel}");
                }
            }
            else
            {
                Debug.Log($"No upgrades available for pool {poolType}.");
            }
        }
    }
}
