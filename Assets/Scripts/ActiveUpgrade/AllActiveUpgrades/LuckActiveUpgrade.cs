using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LuckActiveUpgrade", menuName = "Active Upgrade System/LuckActiveUpgrade")]
public class LuckActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}%");
        
        hero.HeroLuckActiveUpgrade(rareValue.value); 
        
    }
}
