using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DebuffEffectScalerStat", menuName = "Hero Stat System/DebuffEffectScalerStat")]

public class DebuffEffectScalerStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetDeBuffEffectScaler(value);
    }
}
