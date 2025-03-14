using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmorPassiveUpgrade", menuName = "Passive Upgrade System/ArmorPassiveUpgrade")]
public class ArmorPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroArmorPassiveUpgrade(upgradeLevels[level].value); 
    }
}
