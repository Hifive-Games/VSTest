using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegenRateStat", menuName = "Hero Stat System/HealthRegenRateStat")]
public class HealthRegenRateStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetHealthRegenRate(value);
    }
}
