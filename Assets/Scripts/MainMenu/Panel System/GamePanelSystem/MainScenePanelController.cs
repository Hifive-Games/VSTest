using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainScenePanelController : PanelController
{
    // Bu expler gelirken slider biraz saçmalıyo. Expleri aynı anda aldığı için oluyo dotween ile 1 saniyede falan toplama yaparsak işimizi çözer

    [SerializeField] private Image experienceFillImage;
    [SerializeField] private Image healthFillImage;

    [SerializeField] private TextMeshProUGUI currentEXPText;
    [SerializeField] private TextMeshProUGUI maxEXPText;
    [SerializeField] private TextMeshProUGUI currentHPText;
    [SerializeField] private TextMeshProUGUI maxHPText;
    [SerializeField] private TextMeshProUGUI levelText;
    private void OnEnable()
    {
        GameEvents.OnExperienceUpdated += UpdateExperienceSlider;
        GameEvents.OnHealthChanged += UpdateHealthSlider;
        GameEvents.OnLevelUp += UpdateLevelText;
    }

    private void OnDisable()
    {
        GameEvents.OnExperienceUpdated -= UpdateExperienceSlider;
        GameEvents.OnHealthChanged -= UpdateHealthSlider;
        GameEvents.OnLevelUp -= UpdateLevelText;
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
        if (experienceFillImage != null)
        {
            // fillAmount expects 0–1
            experienceFillImage.fillAmount = maxXP > 0 ? (float)currentXP / maxXP : 0f;
            //maxEXPText.text = maxXP.ToString();
            currentEXPText.text = $"{currentXP}/{maxXP}";
        }
    }

    private void UpdateHealthSlider(float currentHP, float maxHp)
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = maxHp > 0f ? currentHP / maxHp : 0f;
            //maxHPText.text = maxHp.ToString();
            currentHPText.text = $"{(int)currentHP}/{maxHp}";
        }
    }

    private void UpdateLevelText()
    {
        levelText.text = TheHeroExperienceManager.Instance.GetCurrentLevel().ToString();
    }
}