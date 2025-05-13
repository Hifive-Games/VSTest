using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AttackDamagePassiveUpgrade", menuName = "Passive Upgrade System/AttackDamagePassiveUpgrade")]

public class AttackDamagePassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroAttackDamagePassiveUpgrade(upgradeLevels[level].value); 
    }
}
