using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedPassiveUpgrade", menuName = "Passive Upgrade System/AttackSpeedPassiveUpgrade")]
public class AttackSpeedPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroAttackSpeedPassiveUpgrade(upgradeLevels[level].value); 
    }
}
