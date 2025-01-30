using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DeBuffEffectScalerActiveUpgrade", menuName = "Active Upgrade System/DeBuffEffectScalerActiveUpgrade")]

public class DeBuffEffectScalerActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}");
        
        hero.HeroDeBuffEffectScalerActiveUpgrade(rareValue.value); 
        
    }
}
