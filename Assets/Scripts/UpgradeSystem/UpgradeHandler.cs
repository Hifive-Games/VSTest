using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHandler : MonoBehaviour
{
    public static UpgradeHandler Instance { get; private set; }
    public List<UpgradeSO> allUpgrades;
    private UpgradeFactory upgradeFactory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        upgradeFactory = new UpgradeFactory(allUpgrades);
    }

    public void LevelUp(UpgradeSO upgrade)
    {
        PlayerU player = new PlayerU();
        player.AddUpgrade(upgrade);
    }

    public List<UpgradeSO> GetAvailableUpgrades(PlayerU player)
    {
        return upgradeFactory.GetAvailableUpgrades(player);
    }

    public void AddUpgrade(UpgradeSO upgrade)
    {
        allUpgrades.Add(upgrade);
    }

    public void RemoveUpgrade(UpgradeSO upgrade)
    {
        allUpgrades.Remove(upgrade);
    }
}

