using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AttackSizePassiveUpgrade", menuName = "Passive Upgrade System/AttackSizePassiveUpgrade")]

public class AttackSizePassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroAttackSizePassiveUpgrade(upgradeLevels[level].value); 
    }
}
