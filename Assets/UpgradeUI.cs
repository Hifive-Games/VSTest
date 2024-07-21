using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI upgradeDescription;
    public Image upgradeImage;

    private Upgrade upgrade;
    

    public void SetUpgrade(Upgrade upgrade)
    {
        upgradeName.text = upgrade.upgradeName;
        upgradeDescription.text = upgrade.upgradeDescription;
        upgradeImage.sprite = upgrade.upgradeSprite;
        upgradeImage.color = GetRarityColor(upgrade.rarity);

        this.upgrade = upgrade;
    }

    private Color GetRarityColor(UpgradeRarity rarity)
    {
        switch (rarity)
        {
            case UpgradeRarity.Common:
                return Color.white;
            case UpgradeRarity.Rare:
                return Color.blue;
            case UpgradeRarity.Epic:
                return Color.magenta;
            case UpgradeRarity.Legendary:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    public void SelectUpgrade()
    {
        //apply the upgrade
        Player.Instance.ApplyUpgrade(upgrade);

        LevelUpManager.Instance.ClearchoosenUpgrades();
        LevelUpManager.Instance.LevelUp();

        if(Player.Instance.StillNeedToLevelUp())
        {
            Player.Instance.LevelUp();
        }
        else
        {
            Time.timeScale = 1f;
            GameManager.Instance.StartGame();
            InterfaceManager.Instance.HideLevelUpUI();
        }
    }
}
