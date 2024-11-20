using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSaveLoadManager : MonoBehaviourSingletonPersistent<FileSaveLoadManager>
{
    private const string moneyIdentifier="PlayerMoney";
    
    #region Level And Value
        public int GetLevelDataFromFile(PassiveUpgradeData PassiveUpgradeData)
        {
            return PlayerPrefs.GetInt(PassiveUpgradeData.Identifier+PassiveUpgradeData.Prefix+
                                      PassiveUpgradeData.name+PassiveUpgradeData.Prefix+
                                      PassiveUpgradeData.LevelPropery);
        }
        public void SetLevelDataFromFile(PassiveUpgradeData PassiveUpgradeData, int level)
        {
            PlayerPrefs.SetInt(PassiveUpgradeData.Identifier+PassiveUpgradeData.Prefix+
                               PassiveUpgradeData.name+PassiveUpgradeData.Prefix+
                               PassiveUpgradeData.LevelPropery,level);
            SaveChanges();
        }
        public float GetValueDataFromFile(PassiveUpgradeData PassiveUpgradeData)
        {
            return PlayerPrefs.GetInt(PassiveUpgradeData.Identifier+PassiveUpgradeData.Prefix+
                                      PassiveUpgradeData.name+PassiveUpgradeData.Prefix+
                                      PassiveUpgradeData.ValuePropery);
        }
        public void SetValueDataFromFile(PassiveUpgradeData PassiveUpgradeData, float value)
        {
            PlayerPrefs.SetFloat(PassiveUpgradeData.Identifier+PassiveUpgradeData.Prefix+
                                 PassiveUpgradeData.name+PassiveUpgradeData.Prefix+
                                 PassiveUpgradeData.ValuePropery,value);
            SaveChanges();
        }
        public void SaveChanges()
        {
            PlayerPrefs.Save();
        }
    #endregion
    
    
    
    // money gibi generic olan 2 sahne arasında kesinlikle kayıt edilenler için bir sistem yok şuan. o yüzden fonksiyonları ları elle yazalım şimdilik
    // money sistemi eklendiğinde burayı düzenlememiz gerekiyor

    public int GetPlayerMoneyDataFromFile()
    {
        return PlayerPrefs.GetInt(moneyIdentifier);
    }
    public void SetPlayerMoneyDataFromFile(int money)
    {
        PlayerPrefs.SetInt(moneyIdentifier,money);
        SaveChanges();
    }
    public bool HasKeyCheckMoney()
    {
        return PlayerPrefs.HasKey(moneyIdentifier);
    }
}
