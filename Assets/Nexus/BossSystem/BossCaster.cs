using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for boss spell casters.
/// </summary>
public class BossCaster : MonoBehaviour, IBossSpellCaster
{
    [Header("Spell Info")]
    [SerializeField] private BossSpell[] _spells = new BossSpell[16];

    [Header("Caster Info")]
    [SerializeField] private Transform _transform;

    /// <summary>
    /// The caster’s transform (position, rotation, etc.).
    /// </summary>
    public Transform Transform => _transform;

    private BossController _bossController;
    private void Awake()
    {
        _bossController = GetComponent<BossController>();
        if (_transform == null)
        {
            _transform = transform;
        }
    }

    /// <summary>
    /// Helper function to Add a BossSpell to this caster.
    /// </summary>
    /// <param name="spell">The spell to add.</param>
    public void AddSpell(BossSpell spell)
    {
        if (spell == null)
        {
            Debug.LogError("Cannot add a null spell.");
            return;
        }

        for (int i = 0; i < _spells.Length; i++)
        {
            if (_spells[i] == null)
            {
                _spells[i] = spell;
                return;
            }
        }
    }

    /// <summary>
    /// Helper function to Remove a BossSpell from this caster.
    /// </summary>
    /// <param name="spell">The spell to remove.</param>
    public void RemoveSpell(BossSpell spell)
    {
        if (spell == null)
        {
            Debug.LogError("Cannot remove a null spell.");
            return;
        }

        for (int i = 0; i < _spells.Length; i++)
        {
            if (_spells[i] == spell)
            {
                _spells[i] = null;
                return;
            }
        }
    }

    /// <summary>
    /// Clear all spells from this caster.
    /// </summary>
    public void ClearSpells()
    {
        for (int i = 0; i < _spells.Length; i++)
        {
            _spells[i] = null;
        }
    }

    /// <summary>
    /// Invoke a BossSpell on this caster.
    /// </summary>
    /// <param name="spell">The spell to cast.</param>
    /// <param name="target">The target to cast the spell on.</param>
    public void Cast(BossSpell spell, Transform target)
    {
        if (spell == null)
        {
            Debug.LogError("Cannot cast a null spell.");
            return;
        }

        // Cast the spell
        spell.Cast(_bossController, target);
        // Reset the cooldown timer
        spell.SetCooldown(spell.cooldown);
        Debug.Log($"Casting spell: {spell.name}");
    }

    /// <summary>
    /// Set the cooldown for a specific spell.
    /// </summary>
    /// <param name="spell">The spell to set the cooldown for.</param>
    /// <param name="cooldown">The cooldown time in seconds.</param>
    public void SetSpellCooldown(BossSpell spell, float cooldown)
    {
        if (spell == null)
        {
            Debug.LogError("Cannot set cooldown for a null spell.");
            return;
        }

        // Set the cooldown for the spell
        spell.SetCooldown(cooldown);
    }

    /// <summary>
    /// Get the spell at the specified index.
    /// </summary>
    /// <param name="index">The index of the spell.</param>
    public BossSpell GetSpell(int index)
    {
        if (index < 0 || index >= _spells.Length)
        {
            Debug.LogError($"Invalid spell index: {index}");
            return null;
        }

        return _spells[index];
    }
}