using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI upgradeDescription;
    public Image upgradeIcon;
    public Button upgradeButton;
    public UpgradeSO upgrade;

    public void SetUpgrade(UpgradeSO upgrade)
    {
        this.upgrade = upgrade;
        upgradeName.text = upgrade.upgradeName;
        upgradeDescription.text = upgrade.description;
        upgradeIcon.sprite = upgrade.icon;
        upgradeButton.onClick.AddListener(() => OnClicked());
    }

    private void OnClicked()
    {
        UpgradeHandler.Instance.LevelUp(upgrade);
        LevelUpPanel.Instance.gameObject.SetActive(false);
    }
}
