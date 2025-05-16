using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AttackSizeBuff", menuName = "BuffDeBuffSystem/Buff/AttackSizeBuff")]

public class AttackSizeBuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDeBuffSystem()
    {
        TheHero.Instance.AddAttackSize(GetBuffValue());
    }
}
