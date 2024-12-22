using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedActiveUpgrade", menuName = "Active Upgrade System/AttackSpeedActiveUpgrade")]
public class AttackSpeedActiveUpgrade : ActiveUpgradeBaseData
{
    public override void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero)
    {
        var rareValue = rareValues.Find(r => r.rareLevel == selectedRare);
        if (rareValue != null)
        {
            Debug.Log($"Applied {selectedRare} Attack Speed Upgrade: +{rareValue.value}%");
        }
        else
        {
            Debug.LogWarning($"No value found for rare level: {selectedRare}");
        }
    }
}
