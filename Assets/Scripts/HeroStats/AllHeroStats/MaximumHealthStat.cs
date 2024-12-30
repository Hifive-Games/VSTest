using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaximumHealthStat", menuName = "Hero Stat System/MaximumHealthStat")]
public class MaximumHealthStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetMaximumHealth(value);
    }
}
