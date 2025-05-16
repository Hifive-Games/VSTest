using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDamageBuff", menuName = "BuffDeBuffSystem/Buff/AttackDamageBuff")]
public class AttackDamageBuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDeBuffSystem()
    {
        TheHero.Instance.AddAttackDamage(GetBuffValue());
    }
}
