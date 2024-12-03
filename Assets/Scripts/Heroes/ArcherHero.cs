using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcherHero", menuName = "CreateHero/ArcherHero")]
public class ArcherHero : HeroBaseData
{
    public override void HeroAttackSpeedPassiveUpgrade(float value)
    {
        Debug.LogError("ArcherHero!");
    }
}
