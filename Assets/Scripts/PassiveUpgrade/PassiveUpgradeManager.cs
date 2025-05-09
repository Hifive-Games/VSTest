using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class PassiveUpgradeManager : MonoBehaviourSingleton<PassiveUpgradeManager>
{
    [SerializeField] private List<PassiveUpgradeBaseData> upgrades = new List<PassiveUpgradeBaseData>();
    [SerializeField] private GameObject PassiveUpgradeUIPrefab; // Yükseltme UI prefabı
    [SerializeField] private Transform PassiveUpgradeUIParent; // UI objeleri için parent
    [SerializeField] private GameObject playerMoneyPrefab; // Player money texti
    [SerializeField] private Transform playerMoneyParent; // Player money texti için parent
    [SerializeField] private GameObject notEnoughMoneyPrefab; // Yetersiz para UI prefabı
    private TextMeshProUGUI playerMoneyText; // Player money text bileşeni
    //private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // Yükseltme seviyeleri
    private List<PassiveUpgradeUI> upgradeUIs = new List<PassiveUpgradeUI>(); // Yükseltme UI bileşenleri
    
    public static event UnityAction<PassiveUpgradeBaseData> OnPassiveUpgradeRequested;
    public static void RequestPassiveUpgrade(PassiveUpgradeBaseData baseData)
    {
        OnPassiveUpgradeRequested?.Invoke(baseData);
    }
    
    private void OnEnable()
    {
        OnPassiveUpgradeRequested += PassiveUpgrade;
    }

    private void OnDisable()
    {
        OnPassiveUpgradeRequested -= PassiveUpgrade;
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
        PassiveUpgradeBaseData[] loadedUpgrades = Resources.LoadAll<PassiveUpgradeBaseData>(ResourcePathManager.Instance.GetPassiveUpgradeDataPath());

        if (upgrades != null && upgrades.Count > 0) { upgrades.Clear(); }
        
        upgrades.AddRange(loadedUpgrades);

        // Yükleme işlemi tamamlandı, diğer işlemlere geçebilirsiniz
        yield return null; // Burada bir bekleme yok, yalnızca Coroutine'den dönüş yapıyoruz.
    }

    private void CreatePlayerMoneyUI()
    {
        GameObject uiObject = Instantiate(playerMoneyPrefab, playerMoneyParent);
        playerMoneyText = uiObject.GetComponent<TextMeshProUGUI>();
    }

    private void LoadUpgrade(PassiveUpgradeBaseData passiveUpgradeBase)
    {
        int savedLevel = FileSaveLoadManager.Instance.GetLevelDataFromFile(passiveUpgradeBase);

        if (savedLevel == 0 && !PlayerPrefs.HasKey(passiveUpgradeBase.Identifier+ passiveUpgradeBase.Prefix+
                                                   passiveUpgradeBase.name + passiveUpgradeBase.Prefix+
                                                   passiveUpgradeBase.LevelPropery))
        {
            FileSaveLoadManager.Instance.SetLevelDataFromFile(passiveUpgradeBase, 0);
        }
    }

    private void SaveUpgrade(PassiveUpgradeBaseData passiveUpgradeBase)
    {
        //int currentLevel = upgradeLevels[passiveUpgrade.upgradeName];
        int currentLevel = FileSaveLoadManager.Instance.GetLevelDataFromFile(passiveUpgradeBase);

        FileSaveLoadManager.Instance.SetLevelDataFromFile(passiveUpgradeBase,currentLevel);
    }

    private void CreateUpgradeUI(PassiveUpgradeBaseData passiveUpgradeBase)
    {
        GameObject uiObject = Instantiate(PassiveUpgradeUIPrefab, PassiveUpgradeUIParent);

        PassiveUpgradeUI ui = uiObject.GetComponent<PassiveUpgradeUI>();
        ui.SetUpgrade(passiveUpgradeBase, PassiveUpgrade); // UpgradeUI'ı ayarla
        upgradeUIs.Add(ui); // UI objesini listeye ekle
    }

    public void PassiveUpgrade(PassiveUpgradeBaseData passiveUpgradeBase)
    {
        //int currentLevel = upgradeLevels[passiveUpgrade.upgradeName];
        int currentLevel = FileSaveLoadManager.Instance.GetLevelDataFromFile(passiveUpgradeBase);
        if (currentLevel < passiveUpgradeBase.upgradeLevels.Count)
        {
            /* Eğer sonraki level için cost değeri check etmemiz gerekirse bunu kullan.
            if (currentLevel < passiveUpgrade.upgradeLevels.Count - 1)
            {
            }
            */
            int cost = passiveUpgradeBase.upgradeLevels[currentLevel].cost;
            
            if (CanAffordUpgrade(cost)) // Maliyet kontrolü
            {
                if (CanApplyUpgrade(passiveUpgradeBase))
                {
                    // Seviye artışı
                    //upgradeLevels[passiveUpgrade.upgradeName]++;
                    FileSaveLoadManager.Instance.SetLevelDataFromFile(passiveUpgradeBase,currentLevel+1);
                    SaveUpgrade(passiveUpgradeBase);
                }
            }
            else
            {
                NotEnoghMoneyUI();
            }
        }
        else
        {
            Debug.Log("Maksimum seviyeye ulaşıldı!");
        }

        UpdatePlayerMoneyUI(); // Yükseltmeden sonra PlayerMoney UI'sını güncelle
        UpdateAllPassiveUpgradeUI();
    }

    private void NotEnoghMoneyUI()
    {
        GameObject uiObject = Instantiate(notEnoughMoneyPrefab, playerMoneyParent);
        uiObject.transform.DOLocalMoveY(-50, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            uiObject.transform.DOLocalMoveY(-100, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Destroy(uiObject);
            });
        });
    }

    private bool CanAffordUpgrade(int cost)
    {
        int playerMoney = FileSaveLoadManager.Instance.GetPlayerMoneyDataFromFile();
        return playerMoney >= cost;
    }

    private bool CanApplyUpgrade(PassiveUpgradeBaseData passiveUpgradeBase)
    {
        // Mevcut seviye
        int currentLevel = FileSaveLoadManager.Instance.GetLevelDataFromFile(passiveUpgradeBase);

        // Eğer son seviyeye ulaşılmamışsa yükseltme yapılabilir
        if (currentLevel < passiveUpgradeBase.upgradeLevels.Count - 1)
        {
            int cost = passiveUpgradeBase.upgradeLevels[currentLevel].cost;
            
            int nextLevel = currentLevel + 1;
            float nextValue = passiveUpgradeBase.upgradeLevels[nextLevel].value;
            
            FileSaveLoadManager.Instance.SetLevelDataFromFile(passiveUpgradeBase, nextLevel);

            int playerMoney = FileSaveLoadManager.Instance.GetPlayerMoneyDataFromFile();
            // int cost = costData.GetCost(costData.currentLevelCostIndex); // Geçerli maliyeti al
            
            playerMoney -= cost; // Oyuncunun parasını azalt
            FileSaveLoadManager.Instance.SetPlayerMoneyDataFromFile(playerMoney);
            PlayerPrefs.Save();
            
            Debug.Log($"{passiveUpgradeBase.upgradeName} yükseltildi. Yeni Seviye: {nextLevel}, Yeni Değer: {nextValue}");
            return true;
        }
        else
        {
            Debug.Log($"{passiveUpgradeBase.upgradeName} maksimum seviyeye ulaştı!");
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
