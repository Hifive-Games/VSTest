using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviourSingletonPersistent<CurrencyManager>
{
    private int money;
    
    public void AddMoney(int m)
    {
        money = FileSaveLoadManager.Instance.GetPlayerMoneyDataFromFile();
        money = m+money;
        FileSaveLoadManager.Instance.SetPlayerMoneyDataFromFile(money);
    }
    public int GetPlayerMoney()
    {
       return FileSaveLoadManager.Instance.GetPlayerMoneyDataFromFile();
    }
    public void  SetPlayerMoney(int money)
    {
        FileSaveLoadManager.Instance.SetPlayerMoneyDataFromFile(money);
    }
    
}
