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


    private PassiveUpgradeBaseData m_PassiveUpgradeBaseData; // UpgradeData referansı
    private System.Action<PassiveUpgradeBaseData> _onUpgradeClicked; // Yükseltme butonuna tıklama olayını temsil eder
    
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
        PassiveUpgradeManager.RequestUpgrade(m_PassiveUpgradeBaseData);
    }
    
    
    public void SetUpgrade(PassiveUpgradeBaseData passiveUpgradeBaseData, System.Action<PassiveUpgradeBaseData> onUpgradeClicked)
    {
        m_PassiveUpgradeBaseData = passiveUpgradeBaseData;
        _onUpgradeClicked = onUpgradeClicked;

        UpdateUI(); // UI'yi güncelle
    }

    public string GetUpgradeName()
    {
        return m_PassiveUpgradeBaseData.upgradeName; // Yükseltme adını döndür
    }

    public void UpdateUI()
    {
        int currentLevel = FileSaveLoadManager.Instance.GetLevelDataFromFile(m_PassiveUpgradeBaseData);
        
        // _upgradeData'nın null olup olmadığını kontrol et
        if (m_PassiveUpgradeBaseData == null)
        {
            Debug.LogError("UpgradeData is null!");
            return;
        }
    
        if (currentLevel < m_PassiveUpgradeBaseData.upgradeLevels.Count)
        {
            PassiveUpgradeLevel levelData = m_PassiveUpgradeBaseData.upgradeLevels[currentLevel];
            currentValueText.text = $"Current: {levelData.value}"; // Geçerli değeri göster
        
            // Sonraki seviyenin değerini kontrol et
            if (currentLevel+1 < m_PassiveUpgradeBaseData.upgradeLevels.Count)
            {
                nextValueText.text = $"Next: {m_PassiveUpgradeBaseData.upgradeLevels[currentLevel + 1].value}"; // Sonraki değer
                costText.text =$"Cost: "+ m_PassiveUpgradeBaseData.upgradeLevels[currentLevel].cost; // sonraki cost
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
        
        
        upgradeNameText.text = m_PassiveUpgradeBaseData.upgradeName; // Yükseltme adını ayarla

        

        
    }

}
