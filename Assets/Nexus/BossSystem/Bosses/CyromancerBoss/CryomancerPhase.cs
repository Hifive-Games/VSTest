using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The Cryomancer's generic phase.
/// </summary>
[CreateAssetMenu(menuName = "Boss/Phase/Cryomancer", fileName = "NewCryomancerPhase")]
public class CryomancerPhase : ScriptableBossPhase
{
    [Header("Attack/Spell (optional)")]
    public bool enableSpells = false;
    private float _spellTimer = 0f;
    public float SpellTimer;
    public List<BossSpell> spells;

    [Header("Buffs")]
    public float damageMultiplier = 1f;

    [Header("Next Phase")]
    [Tooltip("The health percentage at which to transition to the next phase. 100% = full health, 0% = dead")]
    [Range(100f, 0f)]
    public int healthThreshold = 100;
    public ScriptableBossPhase nextPhase;

    private float _currentHealth;
    private float _maxHealth;

    int _origDamage;

    public override void Enter(BossController boss)
    {
        Debug.Log($"Entering phase: {name}");
        // store & apply buffs
        _origDamage = boss.damage;
        boss.damage = Mathf.RoundToInt(boss.damage * damageMultiplier);

        SpellTimer = _spellTimer;

        // store health
        _currentHealth = boss.currentHealth;
        _maxHealth = boss.maxHealth;

        // fill the spell list if Spells are enabled
        if (enableSpells)
        {
            // check if the boss has a spell caster
            if (boss.Caster == null)
            {
                Debug.LogError("Boss does not have a spell caster.");
                return;
            }

            // add spells to the boss's spell caster
            foreach (var spell in spells)
            {
                if (spell != null)
                {
                    boss.Caster.AddSpell(spell);
                }
                else
                {
                    Debug.LogError("Spell is null.");
                }
            }
        }
    }
    public override void Exit(BossController boss)
    {

        // remove buffs
        boss.damage = _origDamage;

        // remove spells from the boss's spell caster
        if (enableSpells)
        {
            // check if the boss has a spell caster
            if (boss.Caster == null)
            {
                Debug.LogError("Boss does not have a spell caster.");
                return;
            }

            // remove spells from the boss's spell caster
            foreach (var spell in spells)
            {
                if (spell != null)
                {
                    boss.Caster.RemoveSpell(spell);
                }
                else
                {
                    Debug.LogError("Spell is null.");
                }
            }
        }
    }
    public override void Tick(BossController boss)
    {
        // check if the boss is able to cast spells
        if (enableSpells)
        {
            CheckSpellCasting(boss);
        }


        _currentHealth = boss.currentHealth;

        float hpPct = _currentHealth / _maxHealth * 100f;
        // check for phase change
        if (hpPct <= healthThreshold)
        {
            // change phase
            boss.PhaseMachine.ChangeState(nextPhase);
        }

        // death
        if (_currentHealth <= 0)
        {
            boss.StateMachine.ChangeState(boss.DyingState);
        }
    }



    // We need a mechanic for the spell casting. For example, use time base check system(if the timer hit 0 we can pull random amount of spells from the list to cast)
    public void CheckSpellCasting(BossController boss)
    {
        // check if the boss is able to cast spells
        if (enableSpells && SpellTimer <= 0f)
        {
            // pick a random spell from the list
            var spell = spells[Random.Range(0, spells.Count)];
            if (spell != null)
            {
                // cast the spell
                boss.Caster.Cast(spell, boss.Player.transform);
                SpellTimer = _spellTimer;
            }
        }
        else
        {
            SpellTimer -= Time.deltaTime;
        }
    }
}