using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    [SerializeField] private HeroBaseData[] Heroes;  // Karakter SO'larını dizi olarak alıyoruz
    private HeroBaseData selectedHero;

    [SerializeField] private List<ActiveUpgradeBaseData> appliedActiveUpgrades;
    
    [SerializeField] private GameObject heroActiveUpgradePrefab; // Player money texti
    [SerializeField] private Transform heroActiveUpgradeParent;
    
    
    public static event UnityAction<ActiveUpgradeBaseData> OnActiveUpgradeRequested;
    
    public static void RequestActiveUpgrade(ActiveUpgradeBaseData baseData)
    {
        OnActiveUpgradeRequested?.Invoke(baseData);
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
    private void ActiveUpgrade(ActiveUpgradeBaseData activeUpgrade)
    {
        activeUpgrade.ApplyUpgrade(RareLevel.Common,selectedHero);
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
        foreach (var upgrade in appliedActiveUpgrades)
        {
            CreateHeroActiveUpgradeUI(upgrade); // UI objelerini oluştur
        }
        
    }
    
    private void CreateHeroActiveUpgradeUI(ActiveUpgradeBaseData activeUpgradeBase )
    {
        GameObject uiObject = Instantiate(heroActiveUpgradePrefab, heroActiveUpgradeParent);
        ActiveUpgradeUI ui = uiObject.GetComponent<ActiveUpgradeUI>();
        ui.SetUpgrade(selectedHero,activeUpgradeBase,RareLevel.Common,ActiveUpgrade); // UpgradeUI'ı ayarla

    }
    

}
