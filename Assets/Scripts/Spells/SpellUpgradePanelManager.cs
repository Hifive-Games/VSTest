using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SpellUpgradePanelManager : MonoBehaviourSingleton<SpellUpgradePanelManager>
{
    public GameObject SpellUpgradePanel;
    public GameObject SpellUpgradeCardParent;
    public GameObject SpellUpgradeCardPrefab;
    private List<SpellUpgrade> SpellUpgrades = new List<SpellUpgrade>();
    private List<SpellUpgrade> AvailableSpellUpgrades = new List<SpellUpgrade>();

    public int MaxSpellUpgradeCardCount = 3;
    private void Awake()
    {
        LoadSpellUpgrades();
    }

    private void LoadSpellUpgrades()
    {
        SpellUpgrade[] spellUpgrades = Resources.LoadAll<SpellUpgrade>("Spell Upgrade Data");
        foreach (var spellUpgrade in spellUpgrades)
        {
            if (!SpellUpgrades.Contains(spellUpgrade))
            {
                SpellUpgrades.Add(spellUpgrade);
            }
        }

        Debug.Log("Loaded " + SpellUpgrades.Count + " spell upgrades.");
    }

    public void AddEquippedSpellUpgrades(Spell spell)
    {
        foreach (var blueprint in SpellUpgrades)
        {
            if (spell.SpellID == blueprint.TargetID)
            {
                // 1) Clone the ScriptableObject so you don’t mutate the asset
                var clone = ScriptableObject.Instantiate(blueprint);
                clone.name = blueprint.name + "_Instance";

                // 2) Add the clone to your runtime list
                AvailableSpellUpgrades.Add(clone);
            }
        }

        Debug.Log($"Added {AvailableSpellUpgrades.Count} spell upgrades for spell ID: {spell.SpellID}");
    }

    //now we can populate the spell upgrade panel with the available upgrades
    public void PopulateSpellUpgradePanel()
    {
        ClearSpellUpgradePanel();

        var eligible = AvailableSpellUpgrades
            .Where(u => u.Level < u.maxUpgrades)
            .OrderBy(_ => UnityEngine.Random.value)
            .Take(MaxSpellUpgradeCardCount);

        foreach (var upgrade in eligible)
        {
            var card = Instantiate(SpellUpgradeCardPrefab, SpellUpgradeCardParent.transform);
            card.GetComponent<SpellUpgradeCard>().SetSpellUpgrade(upgrade);
        }
    }

    public void CheckSpellAvailability()
    {
        for (int i = AvailableSpellUpgrades.Count - 1; i >= 1; i--)
        {
            if (AvailableSpellUpgrades[i].Level >= AvailableSpellUpgrades[i].maxUpgrades)
            {
                Debug.Log($"Removed spell upgrade: {AvailableSpellUpgrades[i].Name}");
                AvailableSpellUpgrades.RemoveAt(i);
            }
        }
    }

    public void ClearSpellUpgradePanel()
    {
        foreach (Transform child in SpellUpgradeCardParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void ShowSpellUpgradePanel()
    {
        if (SpellManager.Instance.EquippedSpells.Count == 0 || AvailableSpellUpgrades.Count == 0)
        {
            Debug.Log("No spells equipped or no available upgrades.");
            return;
        }
        else
        {
            PopulateSpellUpgradePanel();
        }
    }
}
