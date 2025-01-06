using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegenAmountStat", menuName = "Hero Stat System/HealthRegenAmountStat")]

public class HealthRegenAmountStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetHealthRegenAmount(value);
    }
}
