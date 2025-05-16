using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGamePanelManager : PanelManager
{
   public InteractionProgressBar progressBar; // Progress bar referansı
   public InteractionProgressBar GetProgressBar()
   {
      return progressBar;
   }

   public PanelController gameOverPanelController;
   
   private void OnEnable()
   {
      GameEvents.OnZeroHealth += GameOver;
   }
   private void OnDisable()
   {
      GameEvents.OnZeroHealth -= GameOver;

   }
   private void GameOver()
   {
      GameEvents.currentLevel = 1;
      OpenPanel(gameOverPanelController);
   }
   
}
