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
        DescriptionText.text = $"{SpellUpgrade.Description}";
        ValueText.text = ValueTxt();
        //IconImage.sprite = Resources.Load<Sprite>("Icons/" + spellUpgrade.Target.ToString());
    }

    //get spells current value and the value after the upgrade ("now" -> "after")
    private string ValueTxt()
    {
        SpellData spell = SpellManager.Instance.GetSpellInfo(SpellUpgrade);
        if (spell != null)
        {
            float currentValue = spell.GetValue(SpellUpgrade.Target);
            float newValue = 0;

            //if the spell is cooldown or duration or tick interval new value is calculated by subtracting the current value from the new value, otherwise it is calculated by adding the current value to the new value

            if (SpellUpgrade.Target == UpgradeTarget.Cooldown || SpellUpgrade.Target == UpgradeTarget.TickInterval)
            {
                newValue = currentValue - SpellUpgrade.GetValue();
            }
            else
            {
                newValue = currentValue + SpellUpgrade.GetValue();
            }

            // Format the value. Not more than 1 decimal place
            currentValue = Mathf.Round(currentValue * 10f) / 10f;
            newValue = Mathf.Round(newValue * 10f) / 10f;

            return $"{currentValue} -> {newValue}";
        }
        return "N/A";
    }

    private void OnUpgradeButtonClicked()
    {
        if (SpellManager.Instance != null)
        {
            SpellManager.Instance.ApplyUpgrade(SpellUpgrade);

            SpellUpgradePanelManager.Instance.CheckSpellAvalibility();

            ActiveUpgradeManager.Instance.CloseActiveUpgradeUI();
        }
    }
}
