using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeSystem/Effects/ArmorEffect", order = 1)]
public class ArmorEffect : UpgradeEffects
{
    public override void ApplyUpgrade(float value)
    {
        if (player != null)
        {
            player.armor += value;
        }
    }

    public override void RemoveUpgrade(float value)
    {
        

        if (player != null)
        {
            player.armor -= value;
        }
    }
}
