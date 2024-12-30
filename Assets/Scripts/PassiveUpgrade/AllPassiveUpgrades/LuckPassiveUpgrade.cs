using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LuckPassiveUpgrade", menuName = "Passive Upgrade System/LuckPassiveUpgrade")]
public class LuckPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroLuckPassiveUpgrade(upgradeLevels[level].value); 
    }
}
