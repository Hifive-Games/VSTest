using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgradePanelController : PanelController
{
    public override void OpenPanel()
    {
        base.OpenPanel();
        GameEvents.OnGamePaused?.Invoke();
    }
    
    public override void ClosePanel()
    {
        base.ClosePanel();
        GameEvents.OnGameResumed?.Invoke();
        Debug.Log("ActiveUpgradePanelController: ClosePanel() called");
    }
}
