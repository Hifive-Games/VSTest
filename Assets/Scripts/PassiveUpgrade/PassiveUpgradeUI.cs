using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText; // Yükseltme adı metni
    [SerializeField] private TextMeshProUGUI currentValueText; // Geçerli değer metni
    [SerializeField] private TextMeshProUGUI nextValueText; // Sonraki değer metni

    private PassiveUpgradeData m_PassiveUpgradeData; // UpgradeData referansı
    private System.Action<PassiveUpgradeData> _onUpgradeClicked; // Yükseltme butonuna tıklama olayını temsil eder

    public void SetUpgrade(PassiveUpgradeData passiveUpgradeData, System.Action<PassiveUpgradeData> onUpgradeClicked)
    {
        m_PassiveUpgradeData = passiveUpgradeData;
        _onUpgradeClicked = onUpgradeClicked;

        UpdateUI(); // UI'yi güncelle
        upgradeNameText.text = m_PassiveUpgradeData.upgradeName; // Yükseltme adını ayarla
    }

    public string GetUpgradeName()
    {
        return m_PassiveUpgradeData.upgradeName; // Yükseltme adını döndür
    }

    public void UpdateUI()
    {
        int currentLevel = PlayerPrefs.GetInt(m_PassiveUpgradeData.upgradeName + "_Level", 0);
    
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
            if (currentLevel + 1 < m_PassiveUpgradeData.upgradeLevels.Count)
            {
                nextValueText.text = $"Next: {m_PassiveUpgradeData.upgradeLevels[currentLevel + 1].value}"; // Sonraki değer
            }
            else
            {
                currentValueText.text = $"Current: {levelData.value}";
                nextValueText.text = $"Next: -";
                //upgradeButton.interactable = false; // Butonu devre dışı bırak
            }

           
        }
        
    }

    private void OnUpgradeButtonClicked()
    {
        _onUpgradeClicked?.Invoke(m_PassiveUpgradeData); // Yükseltmeyi uygula
    }

    public PassiveUpgradeData GetUpgradeData()
    {
        return m_PassiveUpgradeData;
    }
}
