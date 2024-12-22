using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackRangePassiveUpgrade", menuName = "Passive Upgrade System/AttackRangePassiveUpgrade")]
public class AttackRangePassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroAttackRangePassiveUpgrade(upgradeLevels[level].value); 
    }
}