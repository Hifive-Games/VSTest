using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PassiveUpgradeReverse : MonoBehaviour
{
    private List<PassiveUpgradeData> upgrades;

    [Inject] private PassiveUpgradeManager passiveUpgradeManager;
    private void Start()
    {
        // Tüm PassiveUpgradeData nesnelerini yüklüyoruz
        upgrades = new List<PassiveUpgradeData>(Resources.LoadAll<PassiveUpgradeData>(ResourcePathManager.Instance.GetPassiveUpgradeDataPath()));
        GetComponent<Button>().onClick.AddListener(ReverseAllPassiveUpgradeDatas);
    }

    private void ReverseAllPassiveUpgradeDatas()
    {
        int totalRefund = 0;
        
        foreach (var upgrade in upgrades)
        {
            // Mevcut seviye dosyadan okunuyor
            int currentLevel = FileSaveLoadManager.Instance.GetLevelDataFromFile(upgrade);

            while (currentLevel > 0)
            {
                int refundCost = upgrade.upgradeLevels[currentLevel - 1].cost;
                totalRefund += refundCost;
                
                // Seviye düşürüyoruz ve yeni seviyeyi kaydediyoruz
                currentLevel--;
                FileSaveLoadManager.Instance.SetLevelDataFromFile(upgrade, currentLevel);
            }

            // Seviye 0 olduğunda, başlangıç değerini kaydediyoruz
            FileSaveLoadManager.Instance.SetValueDataFromFile(upgrade, upgrade.upgradeLevels[0].value);
        }
        Debug.Log("Total refund: " + totalRefund);
        
        FileSaveLoadManager.Instance.SetPlayerMoneyDataFromFile(FileSaveLoadManager.Instance.GetPlayerMoneyDataFromFile()+totalRefund);
        
        passiveUpgradeManager.UpdateAllPassiveUpgradeUI();
        passiveUpgradeManager.UpdatePlayerMoneyUI();
    }
    
}