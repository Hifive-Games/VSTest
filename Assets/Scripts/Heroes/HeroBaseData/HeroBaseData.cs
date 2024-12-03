using System.Collections.Generic;
using UnityEngine;

public class HeroBaseData : ScriptableObject
{
    public int id;
    public string characterName;
    public GameObject prefab;
    public Sprite characterImage;
    public bool isSelected = false;
    [SerializeField] private List<PassiveUpgradeBaseData> AppliedPassiveUpgrades;
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
    public virtual void HeroHealthPassiveUpgrade(float value) {}
    public virtual void HeroHpRegenPassiveUpgrade(float value) {}
    public virtual void HeroMovementSpeedPassiveUpgrade(float value) {}
    

}
