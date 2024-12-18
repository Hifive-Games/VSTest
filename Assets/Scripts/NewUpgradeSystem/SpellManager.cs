using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    public List<ISpell> EquippedSpells = new List<ISpell>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        if (upgrade.Type != UpgradeType.Spell) return;

        foreach (ISpell spell in EquippedSpells)
        {
            if (spell.SpellID == upgrade.TargetID || upgrade.TargetID == 0)
            {
                spell.Upgrade(upgrade);
            }
        }
    }

    public void ResetSpells()
    {
        foreach (var spell in EquippedSpells)
        {
            spell.ResetStats();
        }
    }
    public void EquipSpell(ISpell spell)
    {
        if (!EquippedSpells.Contains(spell))
        {
            EquippedSpells.Add(spell);
            // Initialize the spell if necessary
        }
    }
}