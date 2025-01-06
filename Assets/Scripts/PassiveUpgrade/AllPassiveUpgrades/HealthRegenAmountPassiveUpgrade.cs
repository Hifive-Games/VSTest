using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthRegenAmountPassiveUpgrade", menuName = "Passive Upgrade System/HealthRegenAmountPassiveUpgrade")]

public class HealthRegenAmountPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroHealthRegenAmountPassiveUpgrade(upgradeLevels[level].value); 
    }
}
