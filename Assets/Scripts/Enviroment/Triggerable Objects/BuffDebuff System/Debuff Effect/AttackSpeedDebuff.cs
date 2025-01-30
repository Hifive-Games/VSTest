using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AttackSpeedDebuff", menuName = "BuffDebuffSystem/Debuff/AttackSpeedDebuff")]

public class AttackSpeedDebuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDebuffSystem()
    {
        TheHero.Instance.AddAttackSpeed(GetDebuffValue());
    }
}
