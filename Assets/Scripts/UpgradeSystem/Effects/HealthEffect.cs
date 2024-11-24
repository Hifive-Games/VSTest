using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "UpgradeSystem/Effects/HealthEffect", order = 0)]
public class HealthEffect : UpgradeEffects
{
    public override void ApplyUpgrade(float value)
    {
        if (player != null)
        {
            player.maxHealth += (int)value;
            player.currentHealth += (int)value;
        }
    }

    public override void RemoveUpgrade(float value)
    {
        if (player != null)
        {
            player.maxHealth -= (int)value;
            player.currentHealth -= (int)value;
        }
    }
}

