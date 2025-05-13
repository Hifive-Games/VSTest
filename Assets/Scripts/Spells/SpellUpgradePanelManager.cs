using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpellUpgradePanelManager : MonoBehaviourSingleton<SpellUpgradePanelManager>
{
    public GameObject SpellUpgradePanel;
    public GameObject SpellUpgradeCardParent;
    public GameObject SpellUpgradeCardPrefab;
    private List<SpellUpgrade> SpellUpgrades = new List<SpellUpgrade>();
    private List<SpellUpgrade> AvailableSpellUpgrades = new List<SpellUpgrade>();

    public int MaxSpellUpgradeCardCount = 3;
    private void Start()
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
        foreach (var spellUpgrade in SpellUpgrades)
        {
            if (spell.SpellID == spellUpgrade.TargetID && !AvailableSpellUpgrades.Contains(spellUpgrade))
            {
                AvailableSpellUpgrades.Add(spellUpgrade);
            }
        }

        Debug.Log("Added " + AvailableSpellUpgrades.Count + " spell upgrades for spell ID: " + spell.SpellID);
    }

    //now we can populate the spell upgrade panel with the available upgrades
    public void PopulateSpellUpgradePanel()
    {
        ClearSpellUpgradePanel();
        int cardCount = math.min(AvailableSpellUpgrades.Count, MaxSpellUpgradeCardCount);

        int randomIndex = UnityEngine.Random.Range(0, AvailableSpellUpgrades.Count - cardCount + 1);


        for (int i = 0; i < cardCount; i++)
        {
            SpellUpgrade upgrade = AvailableSpellUpgrades[randomIndex + i];
            if (upgrade.Level < upgrade.maxUpgrades)
            {
                GameObject card = Instantiate(SpellUpgradeCardPrefab, SpellUpgradeCardParent.transform);

                SpellUpgradeCard cardScript = card.GetComponent<SpellUpgradeCard>();

                cardScript.SetSpellUpgrade(upgrade);
            }
        }
    }

    public void CheckSpellAvalibility()
    {
        for (int i = AvailableSpellUpgrades.Count - 1; i > 0; i--)
        {
            if (AvailableSpellUpgrades[i].Level >= AvailableSpellUpgrades[i].maxUpgrades)
            {
                Debug.Log("Removed spell upgrade: " + AvailableSpellUpgrades[i].Name + " from available upgrades.");
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
