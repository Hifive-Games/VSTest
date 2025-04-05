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

}
