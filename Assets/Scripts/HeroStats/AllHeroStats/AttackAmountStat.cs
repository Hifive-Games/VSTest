using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackAmountStat", menuName = "Hero Stat System/AttackAmountStat")]
public class AttackAmountStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetAttackAmount(value);
    }
}
