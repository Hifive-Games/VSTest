using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class HeroBaseData : ScriptableObject
{
    public int id;
    public string characterName;
    public GameObject prefab;
    public Sprite characterImage;
    public bool isSelected = false;
    [SerializeField] private List<PassiveUpgradeBaseData> AppliedPassiveUpgrades;

    #region Editor Code
    [Button]
    public void AddAllPassiveUpgradeToAppliedPassiveUpgrades()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Bu işlem yalnızca Play Mode sırasında çalıştırılabilir.");
            return;
        }
        
        // AppliedPassiveUpgrades listesini temizle
        AppliedPassiveUpgrades.Clear();

        // PassiveUpgradeBaseData türünde tüm kaynakları yükle
        PassiveUpgradeBaseData[] loadedUpgrades = Resources.LoadAll<PassiveUpgradeBaseData>(ResourcePathManager.Instance.GetPassiveUpgradeDataPath());

        // Her birini AppliedPassiveUpgrades listesine ekle
        foreach (var upgrade in loadedUpgrades)
        {
            AppliedPassiveUpgrades.Add(upgrade);
        }
    }
    

    #endregion

    /*
     * Her hero, HeroBaseData'dan türeyecek. AppliedPassiveUpgrades eklenen upgradeler
     * oyun başladığında kendi fonksiyonlarını çağıracak.
     * Sonra her karakterin (ArcherHero, HackerHero) içerisinde override edilen
     * fonksiyonlara göre işlem yapacaklar.
     */

    public void RunAllPassiveUpgrades()
    {
        foreach (var upgrade in AppliedPassiveUpgrades)
        {
            upgrade.ApplyUpgrade(this);
        }
    }
    
    public virtual void HeroAttackSpeedPassiveUpgrade(float value)
    {
        Debug.LogError("HeroBaseData!!!");
    }

    public virtual void HeroHealthPassiveUpgrade(float value)
    {
        TheHero.Instance.SetMaximumHealth(value);
    }

    public virtual void HeroHealthRegenAmountPassiveUpgrade(float value)
    {
        TheHero.Instance.SetHealthRegenAmount(value);
    }
    public virtual void HeroHealthRegenRatePassiveUpgrade(float value)
    {
        TheHero.Instance.SetHealthRegenRate(value);
    }
    
    public virtual void HeroMovementSpeedPassiveUpgrade(float value)
    {
        TheHero.Instance.SetMovementSpeed(value);
    }
    public virtual void HeroAttackRangePassiveUpgrade(float value)
    {
        TheHero.Instance.SetAttackRange(value);
    }
    public virtual void HeroAttackSizePassiveUpgrade(float value)
    {
        TheHero.Instance.SetAttackSize(value);
    }
    public virtual void HeroAttackAmountPassiveUpgrade(float value)
    {
        TheHero.Instance.SetAttackAmount(value);
    }
    
    /*
     * Burası Active Upgradeler içim
     */
    [SerializeField] public List<ActiveUpgradeBaseData> activeUpgrades;
    [SerializeField] private float playerLuck = 5f; // Oyuncunun şans değeri
    
    public void TestActiveUpdate()
    {
        RareLevel selectedRare = activeUpgrades[0].GetRandomRareLevel(playerLuck);
        activeUpgrades[0].ApplyUpgrade(selectedRare,this);
    }
}
/*
 public class ActiveUpgradeManager : MonoBehaviour
{
    [SerializeField] private List<ActiveUpgradeBaseData> availableUpgrades; // Seçilebilecek tüm aktif upgradeler
    [SerializeField] private HeroBaseData currentHero;                     // Şu anki kahraman
    [SerializeField] private float playerLuck;                             // Oyuncunun şansı

    public void GrantUpgrade()
    {
        // Rastgele bir upgrade seç
        int randomIndex = Random.Range(0, availableUpgrades.Count);
        var selectedUpgrade = availableUpgrades[randomIndex];

        // Luck değerine göre RareLevel belirle
        RareLevel selectedRare = selectedUpgrade.GetRandomRareLevel(playerLuck);

        // Upgrade uygula
        selectedUpgrade.ApplyUpgrade(selectedRare, currentHero);
    }
}
 */