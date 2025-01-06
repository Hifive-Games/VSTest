using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthPassiveUpgrade", menuName = "Passive Upgrade System/HealthPassiveUpgrade")]

public class MaximumHealthPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroHealthPassiveUpgrade(upgradeLevels[level].value); 
    }
}
