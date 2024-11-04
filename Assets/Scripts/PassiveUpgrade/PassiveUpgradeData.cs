using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Passive Upgrade System/PassiveUpgradeData")]
public class PassiveUpgradeData : ScriptableObject
{
    public string upgradeName; // Yükseltmenin adı
    public List<PassiveUpgradeLevel> upgradeLevels = new List<PassiveUpgradeLevel>(); // Yükseltme seviyeleri

    [HideInInspector] public int currentLevel; // Mevcut seviye
    [HideInInspector] public float currentValue; // Mevcut seviyenin değeri
}

[System.Serializable]
public class PassiveUpgradeLevel
{
    public float value; // Bu seviyedeki değer
    public int cost; // Bu seviyedeki değer

}