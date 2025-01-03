using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade", order = 0)]
public class Upgrade : ScriptableObject
{
    public int TargetID;
    public string Name;
    public string Description;
    public UpgradeTarget Target;
    public int maxUpgrades = 5;
    public List<float> ValuePerLevel = new List<float>();

    public float GetValue(int level)
    {
        if (level < ValuePerLevel.Count)
        {
            return ValuePerLevel[level];
        }
        else
        {
            return ValuePerLevel[ValuePerLevel.Count - 1];
        }
    }
}
public enum UpgradeTarget
{
    Damage,
    Cooldown,
    Range,
    Duration,
    Speed,
    Radius,
    ProjectileCount,
    TickInterval
}