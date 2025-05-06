using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentHealthStat", menuName = "Hero Stat System/CurrentHealthStat")]

public class CurrentHealthStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        Debug.LogError("ApplyStat: " + value);
        hero.HeroSetCurrentHealth(value);
    }
}
