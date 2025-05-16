using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSaveLoadManager : MonoBehaviourSingletonPersistent<FileSaveLoadManager>
{
    #region Level And Value
        public int GetLevelDataFromFile(PassiveUpgradeBaseData passiveUpgradeBaseData)
        {
            return PlayerPrefs.GetInt(passiveUpgradeBaseData.Identifier+passiveUpgradeBaseData.Prefix+
                                      passiveUpgradeBaseData.upgradeName+passiveUpgradeBaseData.Prefix+
                                      passiveUpgradeBaseData.LevelPropery);
        }
        public void SetLevelDataFromFile(PassiveUpgradeBaseData passiveUpgradeBaseData, int level)
        {
            PlayerPrefs.SetInt(passiveUpgradeBaseData.Identifier+passiveUpgradeBaseData.Prefix+
                               passiveUpgradeBaseData.upgradeName+passiveUpgradeBaseData.Prefix+
                               passiveUpgradeBaseData.LevelPropery,level);
            SaveChanges();
        }
        
        public void SaveChanges()
        {
            PlayerPrefs.Save();
        }
    #endregion
    
    
    // money gibi generic olan 2 sahne arasında kesinlikle kayıt edilenler için bir sistem yok şuan. o yüzden fonksiyonları ları elle yazalım şimdilik
    // money sistemi eklendiğinde burayı düzenlememiz gerekiyor
    
    private const string moneyIdentifier="PlayerMoney";

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
