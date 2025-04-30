using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Upgrade", menuName = "SpellUpgrade", order = 0)]
public class SpellUpgrade : ScriptableObject
{
    public int TargetID;
    public string Name;
    public string Description;
    public UpgradeTarget Target;
    public int Level = 0;
    public int maxUpgrades = 5;
    public List<float> ValuePerLevel = new List<float>();

    public float GetValue()
    {
        if (Level < ValuePerLevel.Count)
        {
            return ValuePerLevel[Level];
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