using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ArmorBuff", menuName = "BuffDeBuffSystem/Buff/ArmorBuff")]

public class ArmorBuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDeBuffSystem()
    {
        TheHero.Instance.AddArmor(GetBuffValue());
    }
}
