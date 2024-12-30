using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegenAmountActiveUpgrade", menuName = "Active Upgrade System/HealthRegenAmountActiveUpgrade")]
public class HealthRegenAmountActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}%");
        
        hero.HeroHealthRegenAmountActiveUpgrade(rareValue.value); 
        
    }
}
