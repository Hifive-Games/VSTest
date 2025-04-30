using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpellUpgradeCard : MonoBehaviour
{
    public SpellUpgrade SpellUpgrade;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI ValueText;
    public Button UpgradeButton;
    public Image IconImage;

    private void Start()
    {
        UpgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    public void SetSpellUpgrade(SpellUpgrade spellUpgrade)
    {
        SpellUpgrade = spellUpgrade;
        NameText.text = SpellUpgrade.Name;
        DescriptionText.text = $"{SpellUpgrade.Description} ({SpellUpgrade.GetValue()})";
        ValueText.text = ValueTxt();
        //IconImage.sprite = Resources.Load<Sprite>("Icons/" + spellUpgrade.Target.ToString());
    }

    //get spells current value and the value after the upgrade ("now" -> "after")
    private string ValueTxt()
    {
        SpellData spell = SpellManager.Instance.GetSpellInfo(SpellUpgrade);
        if (spell != null)
        {
            return $"{spell.GetValue(SpellUpgrade.Target)} -> {spell.GetValue(SpellUpgrade.Target) + SpellUpgrade.GetValue()}";
        }
        return "N/A";
    }

    private void OnUpgradeButtonClicked()
    {
        if (SpellManager.Instance != null)
        {
            SpellManager.Instance.ApplyUpgrade(SpellUpgrade);

            SpellUpgradePanelManager.Instance.CheckSpellAvalibility();

            SpellUpgradePanelManager.Instance.HideSpellUpgradePanel();
        }
    }
}
