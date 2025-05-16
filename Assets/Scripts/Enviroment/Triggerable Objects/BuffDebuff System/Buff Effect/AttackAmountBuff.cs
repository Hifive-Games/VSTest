using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AttackAmountBuff", menuName = "BuffDeBuffSystem/Buff/AttackAmountBuff")]

public class AttackAmountBuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDeBuffSystem()
    {
        TheHero.Instance.AddAttackAmount(GetBuffValue());
    }
}
