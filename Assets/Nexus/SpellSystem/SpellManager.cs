using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    public List<Spell> EquippedSpells = new List<Spell>();
    public List<float> spellCooldowns = new List<float>();

    [SerializeField] List<SpellData> spellDataList = new List<SpellData>();

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

    public List<Spell> GetEquippedSpells()
    {
        return EquippedSpells;
    }

    //helper method for the setting spell cooldowns(take SpellData)
    public void SetSpellCooldowns(SpellData spell, float cooldown)
    {
        for (int i = 0; i < EquippedSpells.Count; i++)
        {
            if (EquippedSpells[i].SpellID == spell.SpellID)
            {
                spellCooldowns[i] = cooldown;
                Debug.Log("Setting cooldown for " + EquippedSpells[i].name + " to " + cooldown);
                break;
            }
        }
    }

    //helper method for the setting spell cooldowns(take Spell)
    public void SetSpellCooldowns(Spell spell, float cooldown)
    {
        for (int i = 0; i < EquippedSpells.Count; i++)
        {
            if (EquippedSpells[i].SpellID == spell.SpellID)
            {
                spellCooldowns[i] = cooldown;
                Debug.Log("Setting cooldown for " + EquippedSpells[i].name + " to " + spellCooldowns[i]);
                break;
            }
        }
    }

    public SpellData GetSpellInfo(SpellUpgrade spellUpgrade)
    {
        foreach (SpellData spell in spellDataList)
        {
            if (spell.SpellID == spellUpgrade.TargetID)
            {
                return spell;
            }
        }
        return null;
    }

    public void ApplyUpgrade(SpellUpgrade upgrade)
    {
        foreach (SpellData spell in spellDataList)
        {
            if (spell.SpellID == upgrade.TargetID)
            {
                // Apply the upgrade to the spell data
                spell.Upgrade(upgrade);
            }
        }
    }

    //create an empty spelldata object and add it to the list of spell data
    public void CreateSpellData(Spell spell)
    {
        SpellData spellData = ScriptableObject.CreateInstance<SpellData>();
        spellData.SpellID = spell.SpellID;
        spellData.Name = spell.Name;
        spellData.Description = spell.Description;
        spellData.Icon = spell.Icon;
        spellData.Speed = spell.speed;
        spellData.Damage = spell.damage;
        spellData.Duration = spell.duration;
        spellData.Range = spell.range;
        spellData.Cooldown = spell.cooldown;
        spellData.ExplosionRadius = spell.explosionRadius;
        spellData.ProjectileCount = spell.projectileCount;
        spellData.Radius = spell.radius;
        spellData.TickInterval = spell.tickInterval;

        // Add the new SpellData to the list
        if (!spellDataList.Contains(spellData))
            spellDataList.Add(spellData);
    }

    public void EquipSpell(Spell spell, bool isDisposable = false)
    {
        if (!EquippedSpells.Contains(spell))
        {
            EquippedSpells.Add(spell);
            spellCooldowns.Add(spell.cooldown);

            CreateSpellData(spell);
        }

        if (!isDisposable)
        {
            SpellUpgradePanelManager.Instance.AddEquippedSpellUpgrades(spell);
        }

        //TODO: Later on we will add a check for disposable spells
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
        SpellData _spellData = spellDataList.Find(x => x.SpellID == spell.SpellID);
        spell.Caster = Caster.Player;

        for (int i = 0; i < _spellData.ProjectileCount; i++)
        {
            GameObject spellObject = ObjectPooler.Instance.SpawnFromPool(spell.gameObject, TheHero.Instance.transform.position, Quaternion.identity);
            Spell spellComponent = spellObject.GetComponent<Spell>();

            ApplyData(spellComponent, _spellData);

            spellObject.transform.position = new Vector3(spellObject.transform.position.x, 1f, spellObject.transform.position.z);
            spellComponent.Seek();
            spellComponent.Release();
            yield return new WaitForSeconds(_spellData.TickInterval);
        }
    }

    public void ApplyData(Spell spell, SpellData spellData)
    {
        spell.speed = spellData.Speed;
        spell.damage = spellData.Damage;
        spell.duration = spellData.Duration;
        spell.range = spellData.Range;
        spell.cooldown = spellData.Cooldown;
        spell.explosionRadius = spellData.ExplosionRadius;
        spell.projectileCount = spellData.ProjectileCount;
        spell.radius = spellData.Radius;
        spell.tickInterval = spellData.TickInterval;
    }
}