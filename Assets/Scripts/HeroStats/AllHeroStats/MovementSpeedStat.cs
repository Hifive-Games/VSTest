using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedStat", menuName = "Hero Stat System/MovementSpeedStat")]
public class MovementSpeedStat : HeroStatsBaseData
{
    public override void ApplyStat( HeroBaseData hero)
    {
        hero.HeroSetMovementSpeed(value);
    }
}
