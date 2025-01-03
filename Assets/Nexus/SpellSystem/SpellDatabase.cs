using System.Collections.Generic;
using UnityEngine;

public class SpellDatabase : MonoBehaviour
{
    public static SpellDatabase Instance;

    public List<Spell> Spells = new List<Spell>();

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

    public Spell GetSpell(int id)
    {
        if (id < 0 || id >= Spells.Count) return null;
        else return Spells.Find(spell => spell.SpellID == id);
    }
}