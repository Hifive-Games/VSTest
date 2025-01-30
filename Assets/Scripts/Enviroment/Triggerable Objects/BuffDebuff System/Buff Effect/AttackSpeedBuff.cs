using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedBuff", menuName = "BuffDebuffSystem/Buff/AttackSpeedBuff")]
public class AttackSpeedBuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDebuffSystem()
    {
        TheHero.Instance.AddAttackSpeed(GetBuffValue());
    }
}