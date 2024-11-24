using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RarityWeight
{
    public Rarity rarity;
    public int weight;
}

[CreateAssetMenu(menuName = "UpgradeSystem/Upgrade", order = 0)]
public class GlobalUpgradeSO : UpgradeCommon
{
    [Header("Upgrade Values per Level")]
    public int maximumLevel;
    public List<int> valueforEachLevel;
    public List<int> valueforEachRarityLevel;

    [Header("Upgrade Requirements & Compatibility")]
    public List<GlobalUpgradeSO> preRequesties;
    public List<GlobalUpgradeSO> incompatibleUpgrades;

    [Header("Upgrade Rarity")]

    // Replace Dictionary with a List of RarityWeight pairs
    [Header("Rarity Weights")]
    [Tooltip("Specify weights for each rarity level.")]
    public List<RarityWeight> rarityWeights;
}