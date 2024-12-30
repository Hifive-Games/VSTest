using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LuckStat", menuName = "Hero Stat System/LuckStat")]

public class LuckStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetLuck(value);
    }
}
