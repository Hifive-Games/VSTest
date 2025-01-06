using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CurrentHealthActiveUpgrade", menuName = "Active Upgrade System/CurrentHealthActiveUpgrade")]

public class CurrentHealthActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
       
        Debug.Log($"Applied {selectedRare} {this.GetType().Name} Upgrade: +{rareValue.value}%");
        
        hero.HeroCurrentHealthActiveUpgrade(rareValue.value); 
        
    }
}
