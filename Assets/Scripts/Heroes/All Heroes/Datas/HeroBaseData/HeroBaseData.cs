using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

[System.Serializable]
public class HeroStat
{
    public HeroStatsBaseData heroStatsBaseData;  // HeroStatsBaseData türünde referans
    public float value;             // İlgili value değeri
}
public class HeroBaseData : ScriptableObject
{
    public int id;
    public string characterName;
    public GameObject prefab;
    public Sprite characterImage;
    public bool isSelected = false;

    [SerializeField] public List<HeroStat> heroStatsBaseDatas;  // Her bir öğeyi tutmak için dizi kullanıyoruz
    [SerializeField] private List<PassiveUpgradeBaseData> appliedPassiveUpgrades;
    [SerializeField] private List<ActiveUpgradeBaseData>  appliedActiveUpgrades;
    
    #region Editor Code
    [Button]
    public void AddAllPassiveUpgradeToAppliedPassiveUpgrades()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.DisplayDialog("Hata", "Bu işlem yalnızca Play Mode sırasında çalıştırılabilir.", "Tamam");
        return;

        }
        #endif
    
        // AppliedPassiveUpgrades listesini temizle
        appliedPassiveUpgrades.Clear();

        // PassiveUpgradeBaseData türünde tüm kaynakları yükle
        PassiveUpgradeBaseData[] loadedUpgrades = Resources.LoadAll<PassiveUpgradeBaseData>(ResourcePathManager.Instance.GetPassiveUpgradeDataPath());

        // Her birini AppliedPassiveUpgrades listesine ekle
        foreach (var upgrade in loadedUpgrades)
        {
            appliedPassiveUpgrades.Add(upgrade);
        }

        #if UNITY_EDITOR
                // Değişiklikleri kalıcı hale getirme
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
        #endif
    }

    [Button()]
    public void AddAllActiveUpgradeToAppliedPassiveUpgrades()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.DisplayDialog("Hata", "Bu işlem yalnızca Play Mode sırasında çalıştırılabilir.", "Tamam");
            return;

        }
        #endif
    
        // AppliedPassiveUpgrades listesini temizle
        appliedActiveUpgrades.Clear();

        // PassiveUpgradeBaseData türünde tüm kaynakları yükle
        ActiveUpgradeBaseData[] loadedUpgrades = Resources.LoadAll<ActiveUpgradeBaseData>(ResourcePathManager.Instance.GetActiveUpgradeDataPath());

        // Her birini AppliedPassiveUpgrades listesine ekle
        foreach (var upgrade in loadedUpgrades)
        {
            appliedActiveUpgrades.Add(upgrade);
        }

        #if UNITY_EDITOR
        // Değişiklikleri kalıcı hale getirme
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }
    
    #if UNITY_EDITOR
    // Burası tamamiyle herostatla ilgili kısım. Yenilemesi gerekiyor bu şekilde yeniliyor onvalidate çok kasıyordu onun yerine yazdım
    private bool isProcessing = false;

    private void OnEnable()
    {
        // EditorApplication.update ile sürekli olarak kontrol etmiyoruz
        EditorApplication.update += UpdateStats;
    }

    private void OnDisable()
    {
        // EditorApplication.update'yi devre dışı bırakıyoruz
        EditorApplication.update -= UpdateStats;
    }

    private void UpdateStats()
    {
        // Değişiklik olup olmadığını kontrol ediyoruz
        bool hasChanges = false;

        foreach (var stat in heroStatsBaseDatas)
        {
            // HeroStatsBaseData'nın değerini kontrol ediyoruz
            if (stat.heroStatsBaseData.value != stat.value)
            {
                stat.heroStatsBaseData.value = stat.value;
                hasChanges = true;
            }
        }

        // Eğer değişiklik olduysa, işlemi kaydediyoruz
        if (hasChanges)
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            isProcessing = true; // İşlem yapıldığı için isProcessing'yi true yapıyoruz
        }

        // Eğer işlem yapıldıysa, editor'un update sırasını bekliyoruz
        if (isProcessing)
        {
            isProcessing = false;
            // Gereksiz yere sürekli işlem yapmamayı sağlıyoruz
            EditorApplication.update -= UpdateStats;
        }
    }
#endif



    #endregion

    /*
     * Her hero, HeroBaseData'dan türeyecek. AppliedPassiveUpgrades eklenen upgradeler
     * oyun başladığında kendi fonksiyonlarını çağıracak.
     * Sonra her karakterin (ArcherHero, HackerHero) içerisinde override edilen
     * fonksiyonlara göre işlem yapacaklar.
     */
    public void RunAllHeroStats()
    {
        foreach (var stat in heroStatsBaseDatas)
        {
            stat.heroStatsBaseData.ApplyStat(this);
        }
    }
    
    public void HeroSetCurrentHealth(float value)
    {
        TheHero.Instance.SetCurrentHealth(value);
    }

    public void HeroSetMovementSpeed(float value)
    {
        TheHero.Instance.SetMovementSpeed(value);
    }

    public void HeroSetMaximumHealth(float value)
    {
        TheHero.Instance.SetMaximumHealth(value);
    }

    public void HeroSetHealthRegenAmount(float value)
    {
        TheHero.Instance.SetHealthRegenAmount(value);
    }

    public void HeroSetHealthRegenRate(float value)
    {
        TheHero.Instance.SetHealthRegenRate(value);
    }

    public void HeroSetAttackSpeed(float value)
    {
        TheHero.Instance.SetAttackSpeed(value);
    }

    public void HeroSetAttackRange(float value)
    {
        TheHero.Instance.SetAttackRange(value);
    }
    
    public void HeroSetAttackSize(float value)
    {
        TheHero.Instance.SetAttackSize(value);
    }

    public void HeroSetAttackAmount(float value)
    {
        TheHero.Instance.SetAttackAmount(value);
    }
    public void HeroSetLuck(float value)
    {
        TheHero.Instance.SetLuck(value);
    }
    
    public void RunAllPassiveUpgrades()
    {
        foreach (var upgrade in appliedPassiveUpgrades)
        {
            upgrade.ApplyUpgrade(this);
        }
    }
    
    public virtual void HeroAttackSpeedPassiveUpgrade(float value)
    {
        TheHero.Instance.AddAttackSpeed(value);
    }

    public virtual void HeroHealthPassiveUpgrade(float value)
    {
        TheHero.Instance.AddMaximumHealth(value);
    }

    public virtual void HeroHealthRegenAmountPassiveUpgrade(float value)
    {
        TheHero.Instance.AddHealthRegenAmount(value);
    }
    public virtual void HeroHealthRegenRatePassiveUpgrade(float value)
    {
        TheHero.Instance.AddHealthRegenRate(value);
    }
    
    public virtual void HeroMovementSpeedPassiveUpgrade(float value)
    {
        TheHero.Instance.AddMovementSpeed(value);
    }
    public virtual void HeroAttackRangePassiveUpgrade(float value)
    {
        TheHero.Instance.AddAttackRange(value);
    }
    public virtual void HeroAttackSizePassiveUpgrade(float value)
    {
        TheHero.Instance.AddAttackSize(value);
    }
    public virtual void HeroAttackAmountPassiveUpgrade(float value)
    {
        TheHero.Instance.AddAttackAmount(value);
    }
    public virtual void HeroLuckPassiveUpgrade(float value)
    {
        TheHero.Instance.AddLuck(value);
    }
    
    /*
     * Burası (aşağısı) Active Upgradeler içim
     */

    public void TestActiveUpdate()
    {
        RareLevel selectedRare = appliedActiveUpgrades[0].GetRandomRareLevel(TheHero.Instance.GetLuck()); // burada getluck değerini bu şekilde almamız doğru mu çok emin değilim aslında
        appliedActiveUpgrades[0].ApplyUpgrade(selectedRare,this);
    }

    public List<ActiveUpgradeBaseData> GetAppliedActiveUpgrades()
    {
        return appliedActiveUpgrades;
    }
    public virtual void HeroAttackSpeedActiveUpgrade(float value)
    {
        TheHero.Instance.AddAttackSpeed(value);
    }
    public virtual void HeroAttackAmountActiveUpgrade(float value)
    {
        TheHero.Instance.AddAttackAmount(value);
    }
    public virtual void HeroAttackRangeActiveUpgrade(float value)
    {
        TheHero.Instance.AddAttackRange(value);
    }
    public virtual void HeroCurrentHealthActiveUpgrade(float value)
    {
        TheHero.Instance.AddCurrentHealth(value);
    }
    public virtual void HeroHealthRegenAmountActiveUpgrade(float value)
    {
        TheHero.Instance.AddHealthRegenAmount(value);
    }
    public virtual void HeroHealthRegenRateActiveUpgrade(float value)
    {
        TheHero.Instance.AddHealthRegenRate(value);
    }
    public virtual void HeroMaximumHealthActiveUpgrade(float value)
    {
        TheHero.Instance.AddMaximumHealth(value);
    }
    public virtual void HeroMovementSpeedActiveUpgrade(float value)
    {
        TheHero.Instance.AddMovementSpeed(value);
    }
    public virtual void HeroAttackSizeActiveUpgrade(float value)
    {
        TheHero.Instance.AddAttackSize(value);
    }
    public virtual void HeroLuckActiveUpgrade(float value)
    {
        TheHero.Instance.AddLuck(value);
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