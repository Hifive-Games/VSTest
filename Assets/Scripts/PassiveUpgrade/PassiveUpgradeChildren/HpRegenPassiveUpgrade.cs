using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HpRegenPassiveUpgrade", menuName = "Passive Upgrade System/HpRegenPassiveUpgrade")]

public class HpRegenPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroHpRegenPassiveUpgrade(upgradeLevels[level].value); 
    }
}
