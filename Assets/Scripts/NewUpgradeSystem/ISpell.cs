using System.Collections.Generic;
public interface ISpell
{
    int SpellID { get; }
    string SpellName { get; }
    string SpellDescription { get; }
    int Damage { get; set; }
    float Cooldown { get; set; }
    float Range { get; set; }
    float Duration { get; set; }
    List<Upgrade> appliedUpgrades { get;}
    int BaseDamage { get; }

    void Cast();
    void Upgrade(Upgrade upgrade);
    void ResetStats();
    List<Upgrade> GetAppliedUpgrades();
}