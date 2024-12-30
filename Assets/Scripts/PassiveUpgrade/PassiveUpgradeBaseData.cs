using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public abstract class PassiveUpgradeBaseData : ScriptableObject,IUpgradeIdentifiable
{
    public string upgradeName; // Yükseltmenin adı
    public List<PassiveUpgradeLevel> upgradeLevels = new List<PassiveUpgradeLevel>(); // Yükseltme seviyeleri
    /*
     İnterface kullanmamızın sebebi; Identifier okuyup gerekli ID bilgisini almak. Burada dikkat gerektiren yer;
     bir tane daha  "PassiveUpgradePrefix" gibi bir string tanımlayıp Identifier tam haline ulaştırmak.
    */
    
    public const string PassiveUpgradePrefix = "PassiveUpgrade";
    public string Identifier => $"{PassiveUpgradePrefix}";
    public string LevelPropery => "Level";
    public string Prefix => "_";
    public abstract void ApplyUpgrade(HeroBaseData hero);

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
    string Prefix { get; }

    /*
     * Identifier = istediğimiz başlangıç adı
     * diğerleri de istediğimiz string (elle vermemiz gerekiyor)
     */

}
