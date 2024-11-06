using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PassiveUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText; // Yükseltme adı metni
    [SerializeField] private TextMeshProUGUI currentValueText; // Geçerli değer metni
    [SerializeField] private TextMeshProUGUI nextValueText; // Sonraki değer metni
    [SerializeField] private TextMeshProUGUI costText; // Sonraki değer metni


    private PassiveUpgradeData m_PassiveUpgradeData; // UpgradeData referansı
    private System.Action<PassiveUpgradeData> _onUpgradeClicked; // Yükseltme butonuna tıklama olayını temsil eder
    
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
        PassiveUpgradeManager.RequestUpgrade(m_PassiveUpgradeData);
    }
    
    
    public void SetUpgrade(PassiveUpgradeData passiveUpgradeData, System.Action<PassiveUpgradeData> onUpgradeClicked)
    {
        m_PassiveUpgradeData = passiveUpgradeData;
        _onUpgradeClicked = onUpgradeClicked;

        UpdateUI(); // UI'yi güncelle
    }

    public string GetUpgradeName()
    {
        return m_PassiveUpgradeData.upgradeName; // Yükseltme adını döndür
    }

    public void UpdateUI()
    {
        int currentLevel = FileSaveLoadManager.Instance.GetLevelDataFromFile(m_PassiveUpgradeData);
        
        // _upgradeData'nın null olup olmadığını kontrol et
        if (m_PassiveUpgradeData == null)
        {
            Debug.LogError("UpgradeData is null!");
            return;
        }
    
        if (currentLevel < m_PassiveUpgradeData.upgradeLevels.Count)
        {
            PassiveUpgradeLevel levelData = m_PassiveUpgradeData.upgradeLevels[currentLevel];
            currentValueText.text = $"Current: {levelData.value}"; // Geçerli değeri göster
        
            // Sonraki seviyenin değerini kontrol et
            if (currentLevel+1 < m_PassiveUpgradeData.upgradeLevels.Count)
            {
                nextValueText.text = $"Next: {m_PassiveUpgradeData.upgradeLevels[currentLevel + 1].value}"; // Sonraki değer
                costText.text =$"Cost: "+ m_PassiveUpgradeData.upgradeLevels[m_PassiveUpgradeData.currentLevel].cost; // sonraki cost
            }
            else
            {
                currentValueText.text = $"Current: {levelData.value}";
                nextValueText.text = $"Next: -";
                costText.text = $"Cost: -";
                upgradeButton.interactable = false; // Butonu devre dışı bırak
            }
        
        }
        else
        {
            Debug.LogError("Abicim hata");
        }
        
        
        upgradeNameText.text = m_PassiveUpgradeData.upgradeName; // Yükseltme adını ayarla

        

        
    }

}
