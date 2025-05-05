using UnityEngine;

public interface IBossSpellCaster
{
    Transform Transform { get; }
    void Cast(BossSpell spell, Transform target);
    void AddSpell(BossSpell spell);
    void RemoveSpell(BossSpell spell);
    void ClearSpells();
    void SetSpellCooldown(BossSpell spell, float cooldown);
    BossSpell GetSpell(int index);
}