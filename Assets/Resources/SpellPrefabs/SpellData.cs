using UnityEngine;

public class SpellData : ScriptableObject
{
    public int SpellID;
    public string Name;
    public string Description;
    public Sprite Icon;
    public float Speed = 10f;
    public int Damage = 50;
    public float Duration = 2f;
    public float Range = 10f;
    public float Cooldown = 5f;
    public float ExplosionRadius = 5f;
    public int ProjectileCount = 1;
    public float Radius = 3f;
    public float TickInterval = 0.5f;

    // Add any other properties you need for your spell data here
    public Caster Caster; // Assuming Caster is a class you have defined elsewhere

    public void Upgrade(SpellUpgrade upgrade)
    {
        switch (upgrade.Target)
        {
            case UpgradeTarget.Damage:
                Damage += (int)upgrade.GetValue();
                break;
            case UpgradeTarget.Cooldown:
                Cooldown -= upgrade.GetValue();
                //set spell manager spell cooldowns to the new cooldown value
                SpellManager.Instance.SetSpellCooldowns(this, Cooldown);
                break;
            case UpgradeTarget.Range:
                Range += upgrade.GetValue();
                break;
            case UpgradeTarget.Duration:
                Duration += upgrade.GetValue();
                break;
            case UpgradeTarget.Speed:
                Speed += upgrade.GetValue();
                break;
            case UpgradeTarget.ProjectileCount:
                ProjectileCount += (int)upgrade.GetValue();
                break;
            case UpgradeTarget.Radius:
                Radius += upgrade.GetValue();
                break;
            case UpgradeTarget.TickInterval:
                TickInterval -= upgrade.GetValue();
                break;
        }

        upgrade.Level++;
    }

    public virtual float GetValue(UpgradeTarget target)
    {
        switch (target)
        {
            case UpgradeTarget.Damage:
                return Damage;
            case UpgradeTarget.Cooldown:
                return Cooldown;
            case UpgradeTarget.Range:
                return Range;
            case UpgradeTarget.Duration:
                return Duration;
            case UpgradeTarget.Speed:
                return Speed;
            case UpgradeTarget.ProjectileCount:
                return ProjectileCount;
            case UpgradeTarget.Radius:
                return Radius;
            case UpgradeTarget.TickInterval:
                return TickInterval;
            default:
                return 0f;
        }
    }
}

public enum Caster { Player, Boss, Enemy }
