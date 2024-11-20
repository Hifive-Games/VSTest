using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class PassiveUpgradeManager : MonoBehaviour
{
    [SerializeField] private List<PassiveUpgradeData> upgrades = new List<PassiveUpgradeData>();
    [SerializeField] private GameObject PassiveUpgradeUIPrefab; // Yükseltme UI prefabı
    [SerializeField] private Transform PassiveUpgradeUIParent; // UI objeleri için parent
    [SerializeField] private GameObject playerMoneyPrefab; // Player money texti
    [SerializeField] private Transform playerMoneyParent; // Player money texti için parent
    private TextMeshProUGUI playerMoneyText; // Player money text bileşeni
    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // Yükseltme seviyeleri
    private List<PassiveUpgradeUI> upgradeUIs = new List<PassiveUpgradeUI>(); // Yükseltme UI bileşenleri
    
    public static event UnityAction<PassiveUpgradeData> OnUpgradeRequested;
    public static void RequestUpgrade(PassiveUpgradeData data)
    {
        OnUpgradeRequested?.Invoke(data);
    }
    
    private void OnEnable()
    {
        OnUpgradeRequested += Upgrade;
    }

    private void OnDisable()
    {
        OnUpgradeRequested -= Upgrade;
    }
    private void Start()
    {
        StartCoroutine(InitializeUpgrades());
    }

    private IEnumerator InitializeUpgrades()
    {
        // PassiveUpgradeData'yı yükle
        yield return StartCoroutine(LoadPassiveUpgradeDataFromResources());

        // UI'yı oluştur ve güncelle
        CreatePlayerMoneyUI();
        UpdatePlayerMoneyUI(); // Başlangıçta PlayerMoney UI'sını güncelle

        foreach (var upgrade in upgrades)
        {
            LoadUpgrade(upgrade); // Yükseltme seviyesini yükle
            CreateUpgradeUI(upgrade); // UI objelerini oluştur
        }
    }

    private IEnumerator LoadPassiveUpgradeDataFromResources()
    {
        // Resources klasöründen PassiveUpgradeData türündeki tüm varlıkları yükle
        PassiveUpgradeData[] loadedUpgrades = Resources.LoadAll<PassiveUpgradeData>(ResourcePathManager.Instance.GetPassiveUpgradeDataPath());

        // upgrades listesine tüm yüklü varlıkları ekle
        upgrades.AddRange(loadedUpgrades);

        // Yükleme işlemi tamamlandı, diğer işlemlere geçebilirsiniz
        yield return null; // Burada bir bekleme yok, yalnızca Coroutine'den dönüş yapıyoruz.
    }

    private void CreatePlayerMoneyUI()
    {
        GameObject uiObject = Instantiate(playerMoneyPrefab, playerMoneyParent);
        playerMoneyText = uiObject.GetComponent<TextMeshProUGUI>();
    }

    private void LoadUpgrade(PassiveUpgradeData passiveUpgrade)
    {
        int savedLevel = FileSaveLoadManager.Instance.GetLevelDataFromFile(passiveUpgrade);
        float savedValue = FileSaveLoadManager.Instance.GetValueDataFromFile(passiveUpgrade);

        if (savedLevel == 0 && !PlayerPrefs.HasKey(passiveUpgrade.Identifier+ passiveUpgrade.Prefix+
                                                   passiveUpgrade.name + passiveUpgrade.Prefix+
                                                   passiveUpgrade.LevelPropery))
        {
            FileSaveLoadManager.Instance.SetLevelDataFromFile(passiveUpgrade, 0);
            FileSaveLoadManager.Instance.SetValueDataFromFile(passiveUpgrade, passiveUpgrade.upgradeLevels[0].value);
        }
        else
        {
            passiveUpgrade.currentLevel = savedLevel;
            passiveUpgrade.currentValue = savedValue;
        }

        upgradeLevels[passiveUpgrade.upgradeName] = passiveUpgrade.currentLevel;
    }

    private void SaveUpgrade(PassiveUpgradeData passiveUpgrade)
    {
        int currentLevel = upgradeLevels[passiveUpgrade.upgradeName];
        FileSaveLoadManager.Instance.SetLevelDataFromFile(passiveUpgrade,currentLevel);
    }

    private void CreateUpgradeUI(PassiveUpgradeData passiveUpgrade)
    {
        GameObject uiObject = Instantiate(PassiveUpgradeUIPrefab, PassiveUpgradeUIParent);

        PassiveUpgradeUI ui = uiObject.GetComponent<PassiveUpgradeUI>();
        ui.SetUpgrade(passiveUpgrade, Upgrade); // UpgradeUI'ı ayarla
        upgradeUIs.Add(ui); // UI objesini listeye ekle
    }

    public void Upgrade(PassiveUpgradeData passiveUpgrade)
    {
        int currentLevel = upgradeLevels[passiveUpgrade.upgradeName];

        if (currentLevel < passiveUpgrade.upgradeLevels.Count)
        {
            /* Eğer sonraki level için cost değeri check etmemiz gerekirse bunu kullan.
            if (currentLevel < passiveUpgrade.upgradeLevels.Count - 1)
            {
            }
            */
            int cost = passiveUpgrade.upgradeLevels[passiveUpgrade.currentLevel].cost;
            
            if (CanAffordUpgrade(cost)) // Maliyet kontrolü
            {
                if (ApplyUpgrade(passiveUpgrade))
                {
                    // Seviye artışı
                    upgradeLevels[passiveUpgrade.upgradeName]++;
                    SaveUpgrade(passiveUpgrade);
                }
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
        UpdateAllPassiveUpgradeUI();
    }

    private bool CanAffordUpgrade(int cost)
    {
        int playerMoney = FileSaveLoadManager.Instance.GetPlayerMoneyDataFromFile();
        return playerMoney >= cost;
    }

    private bool ApplyUpgrade(PassiveUpgradeData passiveUpgrade)
    {
        // Mevcut seviye
        int currentLevel = passiveUpgrade.currentLevel;

        // Eğer son seviyeye ulaşılmamışsa yükseltme yapılabilir
        if (currentLevel < passiveUpgrade.upgradeLevels.Count - 1)
        {
            int cost = passiveUpgrade.upgradeLevels[passiveUpgrade.currentLevel].cost;
            
            int nextLevel = currentLevel + 1;
            float nextValue = passiveUpgrade.upgradeLevels[nextLevel].value;

            // Mevcut seviyeyi ve değeri güncelle
            passiveUpgrade.currentLevel = nextLevel;
            passiveUpgrade.currentValue = nextValue;

            FileSaveLoadManager.Instance.SetLevelDataFromFile(passiveUpgrade, nextLevel);
            FileSaveLoadManager.Instance.SetValueDataFromFile(passiveUpgrade, nextValue);

            int playerMoney = FileSaveLoadManager.Instance.GetPlayerMoneyDataFromFile();
            // int cost = costData.GetCost(costData.currentLevelCostIndex); // Geçerli maliyeti al
            
            playerMoney -= cost; // Oyuncunun parasını azalt
            FileSaveLoadManager.Instance.SetPlayerMoneyDataFromFile(playerMoney);
            PlayerPrefs.Save();
            
            Debug.Log($"{passiveUpgrade.upgradeName} yükseltildi. Yeni Seviye: {nextLevel}, Yeni Değer: {nextValue}");
            return true;
        }
        else
        {
            Debug.Log($"{passiveUpgrade.upgradeName} maksimum seviyeye ulaştı!");
            return false;
        }
    }
    
    public void UpdateAllPassiveUpgradeUI()
    {
        foreach (var ui in upgradeUIs)
        {
            ui.UpdateUI();
        }
    }

    public void UpdatePlayerMoneyUI() // bunu ve update ui function unu başka bir class a geçir bi ara
    {
        // Eğer "PlayerMoney" PlayerPrefs'te yoksa, 0 değeri ile oluştur
        if (!FileSaveLoadManager.Instance.HasKeyCheckMoney())
        {
            FileSaveLoadManager.Instance.SetPlayerMoneyDataFromFile(0);
        }
    
        // PlayerMoney'yi al ve UI'ya ata
        int playerMoney = FileSaveLoadManager.Instance.GetPlayerMoneyDataFromFile();
        playerMoneyText.text = $"Money: {playerMoney}"; // Parayı TextMeshPro bileşenine ata
    }

    [Button()]
    public void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        UpdateAllPassiveUpgradeUI();
    }
    
    
}
