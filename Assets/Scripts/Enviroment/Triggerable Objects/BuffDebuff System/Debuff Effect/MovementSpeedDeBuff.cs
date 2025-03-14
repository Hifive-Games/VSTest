using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MovementSpeedDeBuff", menuName = "BuffDeBuffSystem/DeBuff/MovementSpeedDeBuff")]

public class MovementSpeedDeBuff : BuffDebuffSystemBaseData
{
    // BUFF MI YOKSA DEBUFF MI OLDUĞUNU KONTROL EDİP FONKSİYONA DOĞRU OLANI GÖNDERMEN GEREKİYOR
    public override void ApplyBuffDeBuffSystem()
    {
        TheHero.Instance.ReduceMovementSpeed(GetDeBuffValue());
    }
}
