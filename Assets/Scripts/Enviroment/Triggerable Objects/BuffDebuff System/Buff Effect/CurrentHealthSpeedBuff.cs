using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentHealthSpeedBuff", menuName = "BuffDeBuffSystem/Buff/CurrentHealthSpeedBuff")]
public class CurrentHealthSpeedBuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDeBuffSystem()
    {
        TheHero.Instance.AddCurrentHealth(GetBuffValue());
    }
}
