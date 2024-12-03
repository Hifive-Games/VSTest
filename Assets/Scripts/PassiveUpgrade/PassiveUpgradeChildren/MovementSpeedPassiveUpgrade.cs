using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MovementSpeedPassiveUpgrade", menuName = "Passive Upgrade System/MovementSpeedPassiveUpgrade")]

public class MovementSpeedPassiveUpgrade : PassiveUpgradeBaseData
{
    public override void ApplyUpgrade(HeroBaseData hero)
    {
        int level = FileSaveLoadManager.Instance.GetLevelDataFromFile(this);
        hero.HeroMovementSpeedPassiveUpgrade(upgradeLevels[level].value); 
    }
}
