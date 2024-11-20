using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Passive Upgrade System/PassiveUpgradeData")]
public class PassiveUpgradeData : ScriptableObject,IUpgradeIdentifiable
{
    public string upgradeName; // Yükseltmenin adı
    public List<PassiveUpgradeLevel> upgradeLevels = new List<PassiveUpgradeLevel>(); // Yükseltme seviyeleri

    [HideInInspector] public int currentLevel; // Mevcut seviye
    [HideInInspector] public float currentValue; // Mevcut seviyenin değeri

    /*
     İnterface kullanmamızın sebebi; Identifier okuyup gerekli ID bilgisini almak. Burada dikkat gerektiren yer;
     bir tane daha  "PassiveUpgradePrefix" gibi bir string tanımlayıp Identifier tam haline ulaştırmak.
    */
    
    public const string PassiveUpgradePrefix = "PassiveUpgrade";
    public string Identifier => $"{PassiveUpgradePrefix}";
    public string LevelPropery => "Level";
    public string ValuePropery => "Value";

    public string Prefix = "_";

}

[System.Serializable]
public class PassiveUpgradeLevel
{
    public float value; // Bu seviyedeki değer
    public int cost; // Bu seviyedeki değer

}
public interface IUpgradeIdentifiable
{
    string Identifier { get; }
    string LevelPropery{ get; }
    string ValuePropery{ get; }

    
    /*
     * Identifier = istediğimiz başlangıç adı + name
     * diğerleri de istediğimiz string (elle vermemiz gerekiyor)
     */
    
}
