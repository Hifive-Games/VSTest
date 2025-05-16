using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUpgradeCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Image iconImage;

    private SpellUpgrade spellUpgrade;
    private SpellManager spellMgr;

    private void Awake()
    {
        if (upgradeButton == null) Debug.LogError("UpgradeButton not set", this);
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        spellMgr = SpellManager.Instance;
    }

    private void OnDestroy()
    {
        upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
    }

    public void SetSpellUpgrade(SpellUpgrade upgrade)
    {
        spellUpgrade = upgrade ?? throw new System.ArgumentNullException(nameof(upgrade));
        nameText.text = spellUpgrade.Name;
        descriptionText.text = spellUpgrade.Description;
        valueText.text = FormatValueText();
        // iconImage.sprite = … preload or reference directly
        upgradeButton.interactable = true;
    }

    private string FormatValueText()
    {
        var data = spellMgr.GetSpellInfo(spellUpgrade);
        if (data == null) return "N/A";

        float current = data.GetValue(spellUpgrade.Target);
        float delta = spellUpgrade.GetValue();
        float next = (spellUpgrade.Target == UpgradeTarget.Cooldown
                   || spellUpgrade.Target == UpgradeTarget.TickInterval)
                   ? current - delta
                   : current + delta;

        current = Mathf.Round(current * 100f) / 100f;
        next = Mathf.Round(next * 100f) / 100f;
        return $"{current} → {next}";
    }

    private void OnUpgradeButtonClicked()
    {
        if (!spellMgr.ApplyUpgrade(spellUpgrade))
        {
            Debug.LogWarning($"Failed to apply upgrade {spellUpgrade.Name}");
            upgradeButton.interactable = false;
            return;
        }

        // refresh availability & close UI
        SpellUpgradePanelManager.Instance.CheckSpellAvailability();
        ActiveUpgradeManager.Instance.CloseActiveUpgradeUI();
    }
}