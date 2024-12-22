using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShinobiHeroBase", menuName = "CreateHero/ShinobiHeroBase")]
public class ShinobiHeroBase : HeroBaseData
{
    public override void HeroAttackSpeedPassiveUpgrade(float value)
    {
        //base.HeroAttackSpeedPassiveUpgrade(value);
        /*
         Burası syntax için duruyor. Eğer özel bir şekilde
         çağırmak istersem buradan yapabilirim
         */
        ShinobiHero.Instance.SetAttackSpeed(value);
    }
}
