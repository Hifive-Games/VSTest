using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUpgradeManager : MonoBehaviour
{
    
    [SerializeField] private List<PassiveUpgradeData> upgrades = new List<PassiveUpgradeData>();
    [SerializeField] private GameObject PassiveUpgradeUIPrefab; // Yükseltme UI prefabı
    [SerializeField] private Transform PassiveUpgradeUIParent; // UI objeleri için parent
    [SerializeField] private GameObject playerMoneyPrefab; // Player money texti
    [SerializeField] private Transform playerMoneyParent; // Player money texti için parent
    [SerializeField] private GameObject randomUpgradePrefab; // Player money texti
    [SerializeField] private Transform randomUpgradeParent; // Player money texti için parent
    //[SerializeField] private CostData costData; // CostData ScriptableObject'i
    private TextMeshProUGUI playerMoneyText; // Player money text bileşeni
    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // Yükseltme seviyeleri
    private List<PassiveUpgradeUI> upgradeUIs = new List<PassiveUpgradeUI>(); // Yükseltme UI bileşenleri

    
    #region Animation
    
    [HideInInspector]public List<GameObject> upgradeButtons; // Upgrade butonlarının Image bileşenleri
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private Color finalColor = Color.red;
    [SerializeField] private float flashCount  = 3f;
    [SerializeField] private float highlightDuration = 0.1f;
    [SerializeField] private float finalColorDuration = 3f;
    private int originalIndex;
    private PassiveUpgradeData m_SelectedPassiveUpgrade;
    
    
    #endregion

    
    
    private void Start()
    {
        //costData.LoadCostIndex(); // Maliyet indeksini yükle
        LoadPassiveUpgradeDataFromResources();
        CreatePlayerMoneyUI();
        CreateRandomUpgradeUI();
        UpdatePlayerMoneyUI(); // Başlangıçta PlayerMoney UI'sını güncelle

        foreach (var upgrade in upgrades)
        {
            LoadUpgrade(upgrade); // Yükseltme seviyesini yükle
            CreateUpgradeUI(upgrade); // UI objelerini oluştur
        }
    }

    private void LoadPassiveUpgradeDataFromResources()
    {
        // Resources klasöründen PassiveUpgradeData türündeki tüm varlıkları yükle
        PassiveUpgradeData[] loadedUpgrades = Resources.LoadAll<PassiveUpgradeData>("Passive Upgrade Data");

        // upgrades listesine tüm yüklü varlıkları ekle
        upgrades.AddRange(loadedUpgrades);

    }

    private void CreateRandomUpgradeUI()
    {
        GameObject uiObject = Instantiate(randomUpgradePrefab, randomUpgradeParent);
        uiObject.GetComponent<Button>().onClick.AddListener(RandomUpgrade);

    }
    
    private void CreatePlayerMoneyUI()
    {
        GameObject uiObject = Instantiate(playerMoneyPrefab, playerMoneyParent);
        playerMoneyText = uiObject.GetComponent<TextMeshProUGUI>();
    }

    private void LoadUpgrade(PassiveUpgradeData passiveUpgrade)
    {
        string levelKey = passiveUpgrade.upgradeName + "_Level";
        string valueKey = passiveUpgrade.upgradeName + "_Value";
    
        // Eğer PlayerPrefs'te seviye kaydı yoksa yeni kayıt oluştur
        if (!PlayerPrefs.HasKey(levelKey))
        {
            int initialLevel = 0;
            float initialValue = passiveUpgrade.upgradeLevels[initialLevel].value;

            PlayerPrefs.SetInt(levelKey, initialLevel);
            PlayerPrefs.SetFloat(valueKey, initialValue);
            PlayerPrefs.Save();

            passiveUpgrade.currentLevel = initialLevel;
            passiveUpgrade.currentValue = initialValue;
        }
        else
        {
            // Kayıtlı olan seviyeyi ve değeri yükle
            int savedLevel = PlayerPrefs.GetInt(levelKey);
            float savedValue = PlayerPrefs.GetFloat(valueKey);

            passiveUpgrade.currentLevel = savedLevel;
            passiveUpgrade.currentValue = savedValue;
        }

        // Dictionary güncellemesi
        upgradeLevels[passiveUpgrade.upgradeName] = passiveUpgrade.currentLevel;
    }

    private void SaveUpgrade(PassiveUpgradeData passiveUpgrade)
    {
        int currentLevel = upgradeLevels[passiveUpgrade.upgradeName];
        PlayerPrefs.SetInt(passiveUpgrade.upgradeName + "_Level", currentLevel);
        PlayerPrefs.Save();
    }

    private void CreateUpgradeUI(PassiveUpgradeData passiveUpgrade)
    {
        GameObject uiObject = Instantiate(PassiveUpgradeUIPrefab, PassiveUpgradeUIParent);
        
        upgradeButtons.Add(uiObject);
        
        PassiveUpgradeUI ui = uiObject.GetComponent<PassiveUpgradeUI>();
        ui.SetUpgrade(passiveUpgrade, Upgrade); // UpgradeUI'ı ayarla
        upgradeUIs.Add(ui); // UI objesini listeye ekle
    }

    private void Upgrade(PassiveUpgradeData passiveUpgrade)
    {
        int currentLevel = upgradeLevels[passiveUpgrade.upgradeName];

        if (currentLevel < passiveUpgrade.upgradeLevels.Count)
        {
            //int cost = costData.GetCost(costData.currentLevelCostIndex); // Geçerli maliyeti al

            int cost = 10;
            
            if (CanAffordUpgrade(cost)) // Maliyet kontrolü
            {
                ApplyUpgrade(passiveUpgrade);

                // Seviye artışı
                upgradeLevels[passiveUpgrade.upgradeName]++;
                SaveUpgrade(passiveUpgrade);

                // Maliyeti artır
                
                /*
                costData.IncreaseCostIndex(); // Maliyet indeksini artır
                costData.SaveCostIndex(); // Maliyet indeksini kaydet
                */
                
                StartCoroutine(UpgradeAnimationSequence());
                
            }
            else
            {
                Debug.Log("Yetersiz bakiye!");
            }
        }
        else
        {
            Debug.Log("Maksimum seviyeye ulaşıldı!");
        }

        UpdatePlayerMoneyUI(); // Yükseltmeden sonra PlayerMoney UI'sını güncelle
    }

    private bool CanAffordUpgrade(int cost)
    {
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        return playerMoney >= cost;
    }

    private void ApplyUpgrade(PassiveUpgradeData passiveUpgrade)
    {
        // Mevcut seviye
        int currentLevel = passiveUpgrade.currentLevel;

        // Eğer son seviyeye ulaşılmamışsa yükseltme yapılabilir
        if (currentLevel < passiveUpgrade.upgradeLevels.Count - 1)
        {
            int nextLevel = currentLevel + 1;
            float nextValue = passiveUpgrade.upgradeLevels[nextLevel].value;

            // Mevcut seviyeyi ve değeri güncelle
            passiveUpgrade.currentLevel = nextLevel;
            passiveUpgrade.currentValue = nextValue;

            // PlayerPrefs'e kaydet
            string levelKey = passiveUpgrade.upgradeName + "_Level";
            string valueKey = passiveUpgrade.upgradeName + "_Value";

            PlayerPrefs.SetInt(levelKey, nextLevel);
            PlayerPrefs.SetFloat(valueKey, nextValue);
            PlayerPrefs.Save();

            
            int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
            // int cost = costData.GetCost(costData.currentLevelCostIndex); // Geçerli maliyeti al
            int cost = 10;
            playerMoney -= cost; // Oyuncunun parasını azalt
            PlayerPrefs.SetInt("PlayerMoney", playerMoney);
            PlayerPrefs.Save();
            
            Debug.Log($"{passiveUpgrade.upgradeName} yükseltildi. Yeni Seviye: {nextLevel}, Yeni Değer: {nextValue}");
        }
        else
        {
            Debug.Log($"{passiveUpgrade.upgradeName} maksimum seviyeye ulaştı!");
        }
    }
    

    private void UpdateUI(PassiveUpgradeData passiveUpgrade)
    {
        foreach (var ui in upgradeUIs)
        {
            if (ui.GetUpgradeName() == passiveUpgrade.upgradeName)
            {
                ui.UpdateUI();
                break;
            }
        }
    }

    private void UpdatePlayerMoneyUI()
    {
        // Eğer "PlayerMoney" PlayerPrefs'te yoksa, 0 değeri ile oluştur
        if (!PlayerPrefs.HasKey("PlayerMoney"))
        {
            PlayerPrefs.SetInt("PlayerMoney", 0);
        }
    
        // PlayerMoney'yi al ve UI'ya ata
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney");
        playerMoneyText.text = $"Money: {playerMoney}"; // Parayı TextMeshPro bileşenine ata
    }

    public void RandomUpgrade()
    {
        // Maksimum seviyeye ulaşmayan yükseltmeleri filtrele
        List<PassiveUpgradeData> availableUpgrades = new List<PassiveUpgradeData>();

        foreach (var upgrade in upgrades)
        {
            if (upgrade.currentLevel < upgrade.upgradeLevels.Count - 1)
            {
                availableUpgrades.Add(upgrade);
            }
        }

        // Eğer hiç yükseltilebilir upgrade yoksa çık
        if (availableUpgrades.Count == 0)
        {
            Debug.Log("Yükseltilebilecek başka upgrade kalmadı!");
            return;
        }
        
        /*
         buraya, upgradeButtons listesini kontrol edecek kodu yaz
         upgradeButtons içinde upradeUI classının içindeki upgrade data ile  availableUpgrades listesindeki dataları karşılaştır.
         eğer upgradeButtons içinde availableUpgrades listesinde olmayan bir obje varsa sil
        */

        for (int i = upgradeButtons.Count - 1; i >= 0; i--) // Ters döngü, listeyi güvenli bir şekilde silmek için
        {
            PassiveUpgradeData buttonPassiveUpgradeData = upgradeButtons[i].GetComponent<PassiveUpgradeUI>().GetUpgradeData(); // UpgradeUI sınıfındaki upgradeData'yı al
            if (!availableUpgrades.Contains(buttonPassiveUpgradeData)) // Eğer availableUpgrades içinde yoksa
            {
                Debug.Log($"Siliniyor: {buttonPassiveUpgradeData.upgradeName}");
                upgradeButtons.RemoveAt(i); // Listeyi güncelle
            }
        }
        
        
        // Rastgele bir yükseltme seç
        int randomIndex = Random.Range(0, availableUpgrades.Count);
        m_SelectedPassiveUpgrade = availableUpgrades[randomIndex];

        
       originalIndex = upgradeButtons.FindIndex(button => 
            button.GetComponent<PassiveUpgradeUI>().GetUpgradeData() == m_SelectedPassiveUpgrade); 
        
        
        // Seçilen yükseltmeyi uygula
        Upgrade(m_SelectedPassiveUpgrade);
        
    }



    private IEnumerator UpgradeAnimationSequence()
    {
        // Rastgele 6 buton seçimi için bir liste oluştur
        List<Image> randomButtons = new List<Image>();

        for (int i = 0; i < flashCount; i++)
        {
            Image randomButton = upgradeButtons[Random.Range(0, upgradeButtons.Count)].GetComponent<Image>();
            randomButtons.Add(randomButton);
            
            // Highlight yap (renk değişimi)
            randomButton.DOColor(highlightColor, highlightDuration).SetLoops(2, LoopType.Incremental);
            
            // Bekleme
            yield return new WaitForSeconds(highlightDuration * 2);
        }

        // Son butonu al ve kırmızıya çevir
        Image finalButton = upgradeButtons[originalIndex].GetComponent<Image>();
        Color originalColor = finalButton.color;

        finalButton.DOColor(finalColor, 0.5f);

        // 3 saniye bekle
        yield return new WaitForSeconds(finalColorDuration);

        // Eski rengine geri dön
        finalButton.DOColor(originalColor, 0.5f);
        
        // UI'yi güncelle
        UpdateUI(m_SelectedPassiveUpgrade);
        
    }
    
  
    [Button()]
    public void SetMoney()
    {
        PlayerPrefs.SetInt("PlayerMoney", 100); // Örnek para değeri
        PlayerPrefs.Save();
        UpdatePlayerMoneyUI();
    }

    [Button()]
    public void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        foreach (var var in upgrades)
        {
            UpdateUI(var);
        }
    }
    
    
}
