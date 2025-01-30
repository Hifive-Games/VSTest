using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BuffEffectScalerStat", menuName = "Hero Stat System/BuffEffectScalerStat")]

public class BuffEffectScalerStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetBuffEffectScaler(value);
    }
}
