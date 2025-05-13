using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText; // Yükseltme adı metni
    [SerializeField] private TextMeshProUGUI descriptionText; // Açıklama metni
    [SerializeField] private TextMeshProUGUI rareText; // RareLevel metni
    [SerializeField] private TextMeshProUGUI valueText; // Değer metni

    private HeroBaseData heroBaseData;
    private RareLevel rareLevel;

    private ActiveUpgradeBaseData m_ActiveUpgradeBaseData; // UpgradeData referansı
    private System.Action<ActiveUpgradeBaseData, RareLevel> _onActiveUpgradeClicked; // Yükseltme butonuna tıklama olayını temsil eder
    
    [SerializeField] private Button upgradeButton;

    [SerializeField] private RareLevelColorData rareLevelColorData;
    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(UpgradeButtonOnClick);
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveListener(UpgradeButtonOnClick);
    }

    private void UpgradeButtonOnClick()
    {
        // Event'i tetikliyoruz
        ActiveUpgradeManager.RequestActiveUpgrade(m_ActiveUpgradeBaseData, rareLevel);
    }

    public void SetUpgrade(HeroBaseData heroBaseData, ActiveUpgradeBaseData activeUpgradeBaseData, RareLevel rareLevel, System.Action<ActiveUpgradeBaseData, RareLevel> onUpgradeClicked)
    {
        this.heroBaseData = heroBaseData;
        m_ActiveUpgradeBaseData = activeUpgradeBaseData;
        this.rareLevel = rareLevel;
        _onActiveUpgradeClicked = onUpgradeClicked;

        Color rareColor = rareLevelColorData.GetColor(rareLevel);
        GetComponent<Image>().color = rareColor;
        
        UpdateUI(activeUpgradeBaseData, rareLevel); // UI'yi güncelle
    }

    public string GetUpgradeName()
    {
        return m_ActiveUpgradeBaseData.upgradeName; // Yükseltme adını döndür
    }

    public void UpdateUI(ActiveUpgradeBaseData activeUpgradeBaseData, RareLevel selectedRare)
    {
        // `activeUpgradeBaseData` veya `m_ActiveUpgradeBaseData` null ise hata mesajı
        if (m_ActiveUpgradeBaseData == null)
        {
            Debug.LogError("UpgradeData is null!");
            return;
        }

        upgradeNameText.text = m_ActiveUpgradeBaseData.upgradeName; // Yükseltme adını ayarla
        descriptionText.text = m_ActiveUpgradeBaseData.description; // Açıklama metnini ayarla
        rareText.text = $"({selectedRare})"; // RareLevel metnini ayarla
        valueText.text = $"{m_ActiveUpgradeBaseData.rareValues.Find(r => r.rareLevel == selectedRare).value}"; // Değer metnini ayarla
    }
}
