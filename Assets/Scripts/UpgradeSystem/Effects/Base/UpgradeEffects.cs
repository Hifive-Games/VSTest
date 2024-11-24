using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeEffects : ScriptableObject, IUpgradeEffect
{
    public PlayerStats player { get; set; }

    public abstract void ApplyUpgrade(float value);

    public abstract void RemoveUpgrade(float value);

    private void OnEnable() {
        player = PlayerStats.Instance;
    }
}
