using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDamageStat", menuName = "Hero Stat System/AttackDamageStat")]
public class AttackDamageStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetAttackDamage(value);
    }
}
