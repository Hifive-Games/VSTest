using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DeBuffEffectScalerPassiveUpgrade", menuName = "Passive Upgrade System/DeBuffEffectScalerPassiveUpgrade")]

public class DeBuffEffectScalerPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroDeBuffEffectScalerPassiveUpgrade(upgradeLevels[level].value); 
    }
}
