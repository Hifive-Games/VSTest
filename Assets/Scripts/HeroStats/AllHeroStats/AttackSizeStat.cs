using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackSizeStat", menuName = "Hero Stat System/AttackSizeStat")]
public class AttackSizeStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetAttackSize(value);
    }
}
