using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameBackButtonController : BackButtonController
{
    public bool CanContinueToGame = false;
    
    public override void OnBackButtonClick()
    {
        PanelManager.Instance.GoBack();
        if (CanContinueToGame)
        {
            GameEvents.OnGameResumed?.Invoke();
        }
    }
}
