using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/*
 * BURADA using NaughtyAttributes; KULLANAMAZSIN ÇÜNKÜ KENDİ EDİTÖR KODU VAR ADI RAREVALUEDRAWER
 * BURADA YAPTIĞIN HERHANGİ BİR ŞEY GÖRÜNMEZSE EDİTÖR KODUYLA İLGİLİDİR
 * BURADA POSTPROVESSOR VAR. İLK OLUŞTUĞUNDA ResetRareValues FONKSYİONUNU ÇAĞIRMAK İÇİN
 *
 */
public enum RareLevel
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class RareValue
{
    public RareLevel rareLevel; // Common, Uncommon, Rare, etc.
    public float value;         // Upgrade değeri
    public float baseProbability; // Temel çıkma ihtimali (yüzde)
    public float luckFactor;    // Luck ile etkilenecek oran
}

public abstract class ActiveUpgradeBaseData : ScriptableObject
{
    public string upgradeName;
    public string description ="Description";
    
    public List<RareValue> rareValues;
    
    public RareLevel GetRandomRareLevel(float playerLuck)
    {
        float totalWeight = 0f;

        // Toplam ağırlıklı olasılığı hesapla
        foreach (var rareValue in rareValues)
        {
            totalWeight += rareValue.baseProbability * (1 + (playerLuck / 100 * rareValue.luckFactor));
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        // Ağırlıklı seçim
        foreach (var rareValue in rareValues)
        {
            cumulativeWeight += rareValue.baseProbability * (1 + (playerLuck / 100 * rareValue.luckFactor));
            if (randomValue <= cumulativeWeight)
            {
                return rareValue.rareLevel; // Seçilen RareLevel
            }
        }

        Debug.LogError("HATA RARE!");
        return RareLevel.Common;
    }


    // Upgrade'i uygula
    public abstract void ApplyUpgrade(RareLevel selectedRare, HeroBaseData hero);
    
    #region Editor

    public void ResetRareValues()
    {
        // Enum değerlerini al
        var allRareLevels = Enum.GetValues(typeof(RareLevel));

        // Listeyi sıfırla ve yeniden oluştur
        rareValues = new List<RareValue>(allRareLevels.Length);

        // Her enum değerini listeye ekle
        foreach (RareLevel level in allRareLevels)
        {
            // Yeni RareValue nesnesini oluştur
            var rareValue = new RareValue
            {
                rareLevel = level // Enum değeri
            };

            // RareValue sınıfındaki tüm özelliklere varsayılan değerleri atamak için refleksiyon kullan
            foreach (var property in typeof(RareValue).GetProperties())
            {
                if (property.CanWrite)
                {
                    // Varsayılan değeri atamak için 'default' keyword'ü kullanılabilir
                    property.SetValue(rareValue, GetDefaultValue(property.PropertyType));
                }
            }

            rareValues.Add(rareValue);
        }
    }

    // Varsayılan değeri döndüren yardımcı metod
    private object GetDefaultValue(Type type)
    {
        // Eğer değer tipi değeri ise, varsayılan değeri döndür
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
    #endregion
    
  
}