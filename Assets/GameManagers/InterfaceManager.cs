using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    public TMP_Text killCountText;
    Vector3 originalPosition;
    public TMP_Text currentEnemyCountText;
    public TMP_Text maxEnemyCountText;

    public int killCount = 0;

    public GameObject failedUI;
    public GameObject gameOverUI;
    public GameObject levelUpUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        originalPosition = killCountText.transform.position;
    }

    public void ShowLevelUpUI()
    {
        levelUpUI.SetActive(true);
    }

    public void ShowFailedUI()
    {
        failedUI.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    public void HideGameOverUI()
    {
        gameOverUI.SetActive(false);
    }

    public void HideLevelUpUI()
    {
        levelUpUI.SetActive(false);
    }

    public void HideFailedUI()
    {
        failedUI.SetActive(false);
    }

    public void AddkillCount()
    {
        killCount++;

        killCountText.text = "Kill Count: " + killCount;
    }

    public void UpdateEnemyCount(int current, int max)
    {
        currentEnemyCountText.text = "Current Enemies: " + current;
        maxEnemyCountText.text = "Max Enemies: " + max;
    }
    public void CreateKillCountTextShake()
    {
        StartCoroutine(KillCountTextShake(originalPosition));
    }

    private IEnumerator KillCountTextShake(Vector3 originalPosition)
    {
        float shakeDuration = 0.05f;
        float shakeMagnitude = .3f;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / shakeDuration;
            float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;

            x *= shakeMagnitude * damper;
            y *= shakeMagnitude * damper;

            killCountText.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            killCountText.fontSize = 36 + (int)(percentComplete * 10);

            yield return null;
        }

        killCountText.transform.position = originalPosition;
    }


}
