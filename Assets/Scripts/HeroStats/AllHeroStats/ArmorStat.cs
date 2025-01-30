using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ArmorStat", menuName = "Hero Stat System/ArmorStat")]

public class ArmorStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetArmor(value);
    }
}
