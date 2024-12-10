using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShinobiHero", menuName = "CreateHero/ShinobiHero")]
public class ShinobiHero : HeroBaseData
{
    public override void HeroAttackSpeedPassiveUpgrade(float value)
    {
        Debug.LogError("ShinobiHero!");
    }
}
