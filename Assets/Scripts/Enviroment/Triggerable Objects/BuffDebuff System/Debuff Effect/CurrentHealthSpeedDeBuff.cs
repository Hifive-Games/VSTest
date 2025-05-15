using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentHealthSpeedDeBuff", menuName = "BuffDeBuffSystem/DeBuff/CurrentHealthSpeedDeBuff")]
public class CurrentHealthSpeedDeBuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDeBuffSystem()
    {
        TheHero.Instance.ReduceCurrentHealth(GetDeBuffValue());
    }
}
