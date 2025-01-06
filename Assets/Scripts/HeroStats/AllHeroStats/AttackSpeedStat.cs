using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedStat", menuName = "Hero Stat System/AttackSpeedStat")]
public class AttackSpeedStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetAttackSpeed(value);
    }
}
