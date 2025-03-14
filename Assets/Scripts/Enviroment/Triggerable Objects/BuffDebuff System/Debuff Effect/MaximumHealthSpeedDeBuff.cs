using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaximumHealthSpeedDeBuff", menuName = "BuffDeBuffSystem/DeBuff/MaximumHealthSpeedDeBuff")]
public class MaximumHealthSpeedDeBuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDeBuffSystem()
    {
        TheHero.Instance.ReduceMaximumHealth(GetDeBuffValue());
    }
}
