using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeSystem/Effects/SpeedEffect", order = 2)]
public class SpeedUpEffect : UpgradeEffects
{
    public override void ApplyUpgrade(float value)
    {
        if (player != null)
        {
            player.moveSpeed += value;
        }
    }

    public override void RemoveUpgrade(float value)
    {
        if (player != null)
        {
            player.moveSpeed -= value;
        }
    }
}

