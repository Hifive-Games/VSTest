using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AttackAmountPassiveUpgrade", menuName = "Passive Upgrade System/AttackAmountPassiveUpgrade")]
public class AttackAmountPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroAttackAmountPassiveUpgrade(upgradeLevels[level].value); 
    }
}
