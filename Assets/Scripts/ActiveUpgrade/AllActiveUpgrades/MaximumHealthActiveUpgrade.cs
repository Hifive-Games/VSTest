using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MaximumHealthActiveUpgrade", menuName = "Active Upgrade System/MaximumHealthActiveUpgrade")]
public class MaximumHealthActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}%");
        
        hero.HeroMaximumHealthActiveUpgrade(rareValue.value); 
        
    }
}
