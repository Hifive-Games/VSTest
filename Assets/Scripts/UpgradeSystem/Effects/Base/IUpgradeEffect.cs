using UnityEngine;

public interface IUpgradeEffect
{
    PlayerStats player { get; set;}
    void ApplyUpgrade(float value);
    void RemoveUpgrade(float value);
}