using System.Collections.Generic;
using NaughtyAttributes;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    [SerializeField] private HeroBaseData[] Heroes;  // Karakter SO'larını dizi olarak alıyoruz
    private HeroBaseData selectedHero;

    [SerializeField] private List<ActiveUpgradeBaseData> appliedActiveUpgrades;
    
    [SerializeField] private GameObject heroActiveUpgradePrefab; // UI prefab referansı
    [SerializeField] private Transform heroActiveUpgradeParent; // UI prefab parent
    
    public static event UnityAction<ActiveUpgradeBaseData, RareLevel> OnActiveUpgradeRequested;

    public static void RequestActiveUpgrade(ActiveUpgradeBaseData baseData, RareLevel rareLevel)
    {
        OnActiveUpgradeRequested?.Invoke(baseData, rareLevel);
    }

    private void OnDisable()
    {
        OnActiveUpgradeRequested -= ActiveUpgrade;
    }

    private void OnEnable()
    {
        OnActiveUpgradeRequested += ActiveUpgrade;
        GetActiveHero();
        InitializeAppliedActiveUpgrades();
    }

    private void ActiveUpgrade(ActiveUpgradeBaseData activeUpgrade, RareLevel rareLevel)
    {
        // Belirlenen RareLevel ile yükseltmeyi uygula
        activeUpgrade.ApplyUpgrade(rareLevel, selectedHero);
    }

    private void InitializeAppliedActiveUpgrades()
    {
        if (selectedHero == null)
        {
            Debug.LogWarning("Selected hero is null. Cannot initialize applied active upgrades.");
            return;
        }

        appliedActiveUpgrades = selectedHero.GetAppliedActiveUpgrades() ?? new List<ActiveUpgradeBaseData>();
    }

    private void GetActiveHero()
    {
        // PlayerSO'ları Resources klasöründen dinamik olarak yüklüyoruz
        Heroes = Resources.LoadAll<HeroBaseData>(ResourcePathManager.Instance.GetHeroSOPath());
        selectedHero = GetSelectedCharacter();
    }
    
    private HeroBaseData GetSelectedCharacter()
    {
        foreach (var character in Heroes)
        {
            if (character.isSelected)
            {
                return character;
            }
        }
        return null; // Hiçbir karakter seçilmemişse null döndürüyoruz
    }

    [Button()]
    public void LevelUp()
    {
        if (appliedActiveUpgrades == null || appliedActiveUpgrades.Count == 0)
        {
            Debug.LogWarning("No upgrades available for this hero.");
            return;
        }

        // Rastgele 3 yükseltme seç
        var selectedUpgrades = appliedActiveUpgrades.OrderBy(_ => Random.value).Take(2).ToList();

        foreach (var upgrade in selectedUpgrades)
        {
            // Rastgele RareLevel seçimi
            RareLevel randomRareLevel = upgrade.GetRandomRareLevel(TheHero.Instance.GetLuck());
            
            // UI objesi oluştur ve ayarla
            CreateHeroActiveUpgradeUI(upgrade, randomRareLevel);
        }
    }

    private void CreateHeroActiveUpgradeUI(ActiveUpgradeBaseData activeUpgradeBase, RareLevel randomRareLevel)
    {
        GameObject uiObject = Instantiate(heroActiveUpgradePrefab, heroActiveUpgradeParent);
        ActiveUpgradeUI ui = uiObject.GetComponent<ActiveUpgradeUI>();

        // UI objesine rastgele RareLevel ile ayar yap
        ui.SetUpgrade(selectedHero, activeUpgradeBase, randomRareLevel, ActiveUpgrade);
    }
}
