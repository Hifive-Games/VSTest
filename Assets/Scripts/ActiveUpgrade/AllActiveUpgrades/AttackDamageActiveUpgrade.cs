using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AttackDamageActiveUpgrade", menuName = "Active Upgrade System/AttackDamageActiveUpgrade")]
public class AttackDamageActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}");
        
        hero.HeroAttackDamageActiveUpgrade(rareValue.value); 
        
    }
}
