using UnityEngine;
[CreateAssetMenu(fileName = "HealthRegenRatePassiveUpgrade", menuName = "Passive Upgrade System/HealthRegenRatePassiveUpgrade")]

public class HealthRegenRatePassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroHealthRegenRatePassiveUpgrade(upgradeLevels[level].value); 
    }
}
