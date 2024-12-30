using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedActiveUpgrade", menuName = "Active Upgrade System/AttackSpeedActiveUpgrade")]
public class AttackSpeedActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}%");
        
        hero.HeroAttackSpeedActiveUpgrade(rareValue.value); 
        
    }
}
