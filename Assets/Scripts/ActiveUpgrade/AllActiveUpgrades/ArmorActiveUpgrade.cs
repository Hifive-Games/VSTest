using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmorActiveUpgrade", menuName = "Active Upgrade System/ArmorActiveUpgrade")]
public class ArmorActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}");
        
        hero.HeroArmorActiveUpgrade(rareValue.value); 
        
    }
}
