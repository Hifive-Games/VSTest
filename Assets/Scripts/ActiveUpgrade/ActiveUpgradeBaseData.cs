using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RareLevel
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

[System.Serializable]
public class RareValue
{
    /*
     * Buraya ne eklersen ekle: RareValueDrawer sınıfına gidip OnGUI fonksiyonunu düzenlemen lazım yoksa görünmez
     */
    public RareLevel rareLevel; // Common, Uncommon, Rare, etc.
    public float value;         // Upgrade değeri
    public float baseProbability; // Temel çıkma ihtimali (yüzde)
    public float luckFactor;   // Luck ile etkilenecek oran
}


[CreateAssetMenu(fileName = "NewActiveUpgrade", menuName = "Upgrades/Active Upgrade")]
public abstract class ActiveUpgradeBaseData : ScriptableObject
{
    public List<RareValue> rareValues;


    public RareLevel GetRandomRareLevel(float playerLuck)
    {
        // Toplam ağırlıklı olasılığı hesaplamak için
        float totalWeight = 0f;

        // Olasılıkları ayarla
        foreach (var rareValue in rareValues)
        {
            // LuckFactor'a göre şansları ayarlıyoruz
            rareValue.baseProbability *= 1 + (playerLuck / 100 * rareValue.luckFactor);
            totalWeight += rareValue.baseProbability; // Toplam ağırlığı artır
        }

        // Rastgele bir değer oluştur
        float randomValue = Random.Range(0, totalWeight);

        // Ağırlıklı seçim
        float cumulativeWeight = 0f;
        foreach (var rareValue in rareValues)
        {
            cumulativeWeight += rareValue.baseProbability;
            if (randomValue <= cumulativeWeight)
            {
                return rareValue.rareLevel; // Seçilen RareLevel
            }
        }

        // Hata durumunda default değer döndür
        Debug.LogError("HATA RARE!");
        return RareLevel.Common;
    }

    // Upgrade'i uygula
    public abstract void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero);
    
    
    [Button()]
    public void CreateOrReset()
    {
        // Enum değerlerini al
        var allRareLevels = System.Enum.GetValues(typeof(RareLevel));
    
        // Listeyi sıfırla ve yeniden oluştur
        rareValues = new List<RareValue>(allRareLevels.Length);

        // Her enum değerini listeye ekle
        foreach (RareLevel level in allRareLevels)
        {
            rareValues.Add(new RareValue
            {
                rareLevel = level,          // Enum değeri
                value = 0f,                 // Varsayılan değer
                baseProbability = 0f,       // Varsayılan olasılık
                luckFactor = 0f             // Varsayılan luck faktörü
            });
        }
    }
    
}