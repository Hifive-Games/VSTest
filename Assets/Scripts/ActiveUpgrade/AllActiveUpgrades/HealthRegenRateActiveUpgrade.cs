using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegenRateActiveUpgrade", menuName = "Active Upgrade System/HealthRegenRateActiveUpgrade")]
public class HealthRegenRateActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}%");
        
        hero.HeroHealthRegenRateActiveUpgrade(rareValue.value); 
        
    }
}
