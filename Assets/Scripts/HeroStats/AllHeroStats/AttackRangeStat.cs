using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackRangeStat", menuName = "Hero Stat System/AttackRangeStat")]
public class AttackRangeStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetAttackRange(value);
    }
}
