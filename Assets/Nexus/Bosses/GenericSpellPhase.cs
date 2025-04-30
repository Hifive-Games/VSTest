using UnityEngine;
using BossSystem;
using System.Collections.Generic;
using System.Collections;
public class GenericSpellPhase : IBossPhase
{
    public BossPhaseType PhaseType => BossPhaseType.Spell;
    BossSpellManager bossSpellManager;
    GameObject player;
    public void EnterPhase(BossController controller)
    {
        
        player = GameObject.FindObjectOfType<TheHero>().gameObject;
        bossSpellManager = new BossSpellManager(player);
        //pick 3 random spells from SpellDatabase
        for (int i = 0; i < 4; i++)
        {
            //int randomSpellIndex = Random.Range(0, SpellDatabase.Instance.Spells.Count);
            bossSpellManager.AddSpell(SpellDatabase.Instance.GetSpell(i));
        }
        Debug.Log("Entering GenericSpellPhase");
    }
    public void UpdatePhase(BossController controller)
    {
        bossSpellManager.Update();
    }
    public void ExitPhase(BossController controller)
    {
        Debug.Log("Exiting GenericSpellPhase");
    }
}

public class BossSpellManager
{
    public List<Spell> EquippedSpells = new List<Spell>();
    [SerializeField] private List<float> spellCooldowns = new List<float>();

    private GameObject player;
    public BossSpellManager(GameObject player)
    {
        this.player = player;
    }

    public void AddSpell(Spell spell)
    {
        EquippedSpells.Add(spell);
        spellCooldowns.Add(spell.cooldown);
    }
    public void Update()
    {
        for (int i = 0; i < EquippedSpells.Count; i++)
        {
            spellCooldowns[i] -= Time.deltaTime;
            if (spellCooldowns[i] <= 0f)
            {
                CastSpell(i);
                Debug.Log($"Casting spell {EquippedSpells[i].name}");
                spellCooldowns[i] = EquippedSpells[i].cooldown;
            }
        }
    }

    public void CastSpell(int index)
    {
        Spell spell = EquippedSpells[index];
        if (spell != null)
        {
            spell.Caster = Caster.Boss;
            GameObject spellObject = ObjectPooler.Instance.SpawnFromPool(spell.gameObject, ExampleBoss.Instance.gameObject.transform.position, Quaternion.identity);
            spellObject.GetComponent<Spell>().Seek(player.transform);
            spellObject.GetComponent<Spell>().Release();
            spell.speed = 10;
            spellObject.transform.position = new Vector3(spellObject.transform.position.x, 1f, spellObject.transform.position.z);
        }
    }
}