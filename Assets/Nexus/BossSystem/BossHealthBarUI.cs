using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    public static BossHealthBarUI Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Image healthBarImage;
    public TextMeshProUGUI BossNameText;
    public TextMeshProUGUI BossPhaseText; //it will be used to show the boss phase like x1, x2, x3, etc.(X3 is the first phase of the boss)

    public GameObject healthBarUI;
    public GameObject healthBarUIBG;
    public GameObject bossNameUI;
    public GameObject bossPhaseUI;

    private void Start()
    {
        ActivateHealthBarUI(false);
    }

    void OnDisable()
    {
        // Reset the health bar when the object is disabled
        SetHealthBar(1f); // Full health
        SetBossName(string.Empty);
        SetBossPhase(0); // Reset the boss phase text
        ShowHealthBarUI(false);
        ShowBossNameUI(false);
        ShowBossPhaseUI(false);
    }

    public void ActivateHealthBarUI(bool show)
    {
        healthBarUI.SetActive(show);
        healthBarUIBG.SetActive(show);
        bossNameUI.SetActive(show);
        bossPhaseUI.SetActive(show);
        if (show)
        {
            // Set the initial health bar to full
            SetHealthBar(1f); // Full health
            SetBossName(string.Empty);
            SetBossPhase(0); // Reset the boss phase text
        }
    }

    public void ShowHealthBarUI(bool show)
    {
        healthBarUI.SetActive(show);
    }

    public void ShowBossNameUI(bool show)
    {
        bossNameUI.SetActive(show);
    }
    public void ShowBossPhaseUI(bool show)
    {
        bossPhaseUI.SetActive(show);
    }
    public void SetHealthBar(float healthPercentage)
    {
        // Clamp the health percentage between 0 and 1
        healthPercentage = Mathf.Clamp01(healthPercentage);
        // Set the fill amount of the health bar image
        healthBarImage.fillAmount = healthPercentage;
    }
    public void SetBossName(string name)
    {
        BossNameText.text = name;
    }
    public void SetBossPhase(int phase)
    {
        if (phase != 0)
            BossPhaseText.text = $"Pashe {phase}"; // Set the boss phase text
        else
            BossPhaseText.text = string.Empty; // Clear the boss phase text if phase is 0
    }
}
