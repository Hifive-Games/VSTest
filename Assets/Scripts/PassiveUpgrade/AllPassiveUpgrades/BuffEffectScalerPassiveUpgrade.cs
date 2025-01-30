using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffEffectScalerPassiveUpgrade", menuName = "Passive Upgrade System/BuffEffectScalerPassiveUpgrade")]
public class BuffEffectScalerPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroBuffEffectScalerPassiveUpgrade(upgradeLevels[level].value); 
    }
}
