using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackAmountActiveUpgrade", menuName = "Active Upgrade System/AttackAmountActiveUpgrade")]
public class AttackAmountActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}%");
        
        hero.HeroAttackAmountActiveUpgrade(rareValue.value); 
        
    }
}
