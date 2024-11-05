using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSaveLoadSystem : MonoBehaviour
{
    
    
    #region ProtectedDatas
        public float GetDataLevelFromFile(PassiveUpgradeData PassiveUpgradeData)
        {
            return PlayerPrefs.GetInt(PassiveUpgradeData.Identifier+
                                      PassiveUpgradeData.name+
                                      PassiveUpgradeData.LevelPropery);
        }
        public float GetDataValueFromFile(PassiveUpgradeData PassiveUpgradeData)
        {
            return PlayerPrefs.GetInt(PassiveUpgradeData.Identifier+
                                      PassiveUpgradeData.name+
                                      PassiveUpgradeData.ValuePropery);
        }
        public void SetDataLevelFromFile(PassiveUpgradeData PassiveUpgradeData, int level)
        {
            PlayerPrefs.SetInt(PassiveUpgradeData.Identifier+
                               PassiveUpgradeData.name+
                               PassiveUpgradeData.LevelPropery,level);
        }
        public void SetDataValueFromFile(PassiveUpgradeData PassiveUpgradeData, float value)
        {
            PlayerPrefs.SetFloat(PassiveUpgradeData.Identifier+
                                 PassiveUpgradeData.name+
                                 PassiveUpgradeData.ValuePropery,value);
        }
    #endregion
    // money gibi generic olan 2 sahne arasında kesinlikle kayıt edilenler için bir sistem yok şuan. o yüzden fonksiyonları ları elle yazalım şimdilik
    
    
}
