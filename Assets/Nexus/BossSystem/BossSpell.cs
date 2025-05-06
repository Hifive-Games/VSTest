using UnityEngine;

/// <summary>
/// Base class for boss spells.
/// </summary>
public abstract class BossSpell : ScriptableObject
{
    [Header("Spell Info")]
    public string spellName;
    public float cooldown = 1f;
    public float castTime = 0f;
    public float range = 10f;
    public float duration = 1f;
    public float damage = 0f;

    public bool haveChainSpell = false;
    public BossSpell[] chainSpells;
    /// <summary>
    /// Executes the spell logic on the given boss.
    /// </summary>
    public abstract void Cast(BossController boss, Transform target);
    /// <summary>
    /// Returns the Ready state of this spell.
    /// </summary>
    public bool IsReady(float cooldownTimer)
    {
        return cooldownTimer <= 0f;
    }
    /// <summary>
    /// Returns the cooldown of this spell.
    /// </summary>
    /// <param name="spell">The spell to check.</param>
    public float GetCooldown(BossSpell spell)
    {
        return spell.cooldown;
    }
    /// <summary>
    /// Sets the cooldown of this spell.
    /// </summary>
    /// <param name="spell">The spell to set.</param>
    /// <param name="cooldown">The cooldown to set.</param>
    public void SetCooldown(float cooldown)
    {
        this.cooldown = cooldown;
    }
    /// <summary>
    /// Checks and Casts the chain spell if available.
    /// </summary>
    /// <param name="boss">The boss to cast the spell on.</param>
    /// <param name="target">The target to cast the spell on.</param>
    
    public void CastChainSpell(BossController boss, Transform target)
    {
        if (haveChainSpell && chainSpells != null && chainSpells.Length > 0)
        {
            foreach (var spell in chainSpells)
            {
                if (spell != null)
                {
                    spell.Cast(boss, target);
                }
            }
        }
    }
}