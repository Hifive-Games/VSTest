using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShinobiHeroBase", menuName = "CreateHero/ShinobiHeroBase")]
public class ShinobiHeroBase : HeroBaseData
{
    public override void HeroAttackSpeedPassiveUpgrade(float value)
    {
        base.HeroAttackSpeedPassiveUpgrade(value);
        /*
         Burası syntax için duruyor. Eğer özel bir şekilde
         çağırmak istersem buradan yapabilirim
         
        ShinobiHero.Instance.AddAttackSpeed(value);
        
        mesela bu hero attackspeed değeri normalden daha fazla artıyorsa ona göre işlem yaparız
        her heronun özel durumu için geçerli
        */
    }
}
