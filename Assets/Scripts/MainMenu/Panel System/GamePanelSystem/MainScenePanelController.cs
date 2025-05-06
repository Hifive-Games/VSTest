using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainScenePanelController : PanelController
{
    // Bu expler gelirken slider biraz saçmalıyo. Expleri aynı anda aldığı için oluyo dotween ile 1 saniyede falan toplama yaparsak işimizi çözer
    
    [SerializeField] private Slider experienceSlider;
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        GameEvents.OnExperienceUpdated += UpdateExperienceSlider;
        GameEvents.OnHealthChanged += UpdateHealthSlider;
    }

    private void OnDisable()
    {
        GameEvents.OnExperienceUpdated -= UpdateExperienceSlider;
        GameEvents.OnHealthChanged -= UpdateHealthSlider;
    }

    public override void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public override void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void UpdateExperienceSlider(int currentXP, int maxXP)
    {
        if (experienceSlider != null)
        {
            experienceSlider.maxValue = maxXP;
            experienceSlider.value = currentXP;
        }
    }
    private void UpdateHealthSlider(float currentHP, float maxHp)
    {
        Debug.LogError("Cureent: " + currentHP);
        Debug.LogError("Max: " + maxHp);
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHp;
            healthSlider.value = currentHP;
        }
    }
}