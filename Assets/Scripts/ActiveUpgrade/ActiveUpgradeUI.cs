using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class ActiveUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText; // Yükseltme adı metni
    [SerializeField] private TextMeshProUGUI descriptionText; // Geçerli değer metni
    [SerializeField] private TextMeshProUGUI rareText; // Sonraki değer metni
    [SerializeField] private TextMeshProUGUI valueText; // Sonraki değer metni

    private HeroBaseData heroBaseData;
    private RareLevel rareLevel;
    
    private ActiveUpgradeBaseData m_ActiveUpgradeBaseData; // UpgradeData referansı
    private System.Action<ActiveUpgradeBaseData> _onActiveUpgradeClicked; // Yükseltme butonuna tıklama olayını temsil eder
    
    [SerializeField] private Button upgradeButton;
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
        LevelManager.RequestActiveUpgrade(m_ActiveUpgradeBaseData);
    }
    
    
    public void SetUpgrade(HeroBaseData heroBaseData,ActiveUpgradeBaseData activeUpgradeBaseData, RareLevel rareLevel, System.Action<ActiveUpgradeBaseData> onUpgradeClicked)
    {
        m_ActiveUpgradeBaseData = activeUpgradeBaseData;
        _onActiveUpgradeClicked = onUpgradeClicked;

        UpdateUI(activeUpgradeBaseData,rareLevel); // UI'yi güncelle
    }

    public string GetUpgradeName()
    {
        return m_ActiveUpgradeBaseData.upgradeName; // Yükseltme adını döndür
    }

    public void UpdateUI(ActiveUpgradeBaseData activeUpgradeBaseData, RareLevel selectedRare)
    {
        // _upgradeData'nın null olup olmadığını kontrol et
        if (m_ActiveUpgradeBaseData == null)
        {
            Debug.LogError("UpgradeData is null!");
            return;
        }
    
        upgradeNameText.text = m_ActiveUpgradeBaseData.upgradeName; // Yükseltme adını ayarla

        descriptionText.text = m_ActiveUpgradeBaseData.description;

        rareText.text = selectedRare.ToString();
        
        valueText.text = activeUpgradeBaseData.rareValues.Find(r => r.rareLevel == selectedRare).value.ToString();
    }
    
}
