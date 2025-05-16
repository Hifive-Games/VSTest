using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameBackButtonController : BackButtonController
{
    public bool CanContinueToGame = false;

    public GameObject LevelUpPanel;

    public override void OnBackButtonClick()
    {
        PanelManager.Instance.GoBack();
        if (LevelUpPanel.activeSelf)
        {
            return;
        }
        if (CanContinueToGame)
        {
            GameEvents.OnGameResumed?.Invoke();
        }
    }
}
