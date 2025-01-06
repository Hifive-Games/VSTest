using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    public List<Spell> EquippedSpells = new List<Spell>();
    [SerializeField] private List<float> spellCooldowns = new List<float>();

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

    private void Start()
    {
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        foreach (Spell spell in EquippedSpells)
        {
            if (spell.SpellID == upgrade.TargetID)
            {
                spell.Upgrade(upgrade);
            }
        }
    }
    public void EquipSpell(Spell spell)
    {
        if (!EquippedSpells.Contains(spell))
        {
            EquippedSpells.Add(spell);
            spellCooldowns.Add(spell.cooldown);
        }
    }

    private void Update()
    {
        for (int i = 0; i < EquippedSpells.Count; i++)
        {
            spellCooldowns[i] -= Time.deltaTime;
            if (spellCooldowns[i] <= 0f)
            {
                StartCoroutine(CastSpell(i));
                spellCooldowns[i] = EquippedSpells[i].cooldown;
            }
        }
    }

    public IEnumerator CastSpell(int index)
    {
        Spell spell = EquippedSpells[index];
        for (int i = 0; i < spell.projectileCount; i++)
        {
            GameObject spellObject = ObjectPooler.Instance.SpawnFromPool(spell.gameObject, Player.Instance.gameObject.transform.position, Quaternion.identity);
            spellObject.transform.position = new Vector3(spellObject.transform.position.x, 1f, spellObject.transform.position.z);
            yield return new WaitForSeconds(spell.tickInterval);
        }
    }
}