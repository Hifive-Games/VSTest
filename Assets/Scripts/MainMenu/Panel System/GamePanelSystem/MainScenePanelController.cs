using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScenePanelController : PanelController
{
    // Bu expler gelirken slider biraz saçmalıyo. Expleri aynı anda aldığı için oluyo dotween ile 1 saniyede falan toplama yaparsak işimizi çözer
    
    [SerializeField] private Slider levelSlider;

    private void OnEnable()
    {
        GameEvents.OnExperienceUpdated += UpdateSlider;
    }

    private void OnDisable()
    {
        GameEvents.OnExperienceUpdated -= UpdateSlider;
    }

    public override void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public override void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void UpdateSlider(int currentXP, int maxXP)
    {
        if (levelSlider != null)
        {
            levelSlider.maxValue = maxXP;
            levelSlider.value = currentXP;
        }
    }
}