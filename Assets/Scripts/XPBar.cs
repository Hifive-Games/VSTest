using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
    //this cs will be attached the raw image ui object, and will be act as the xp bar

    public RectTransform xpBar;
    public float fillSpeed = 0.5f;
    public float currentXP = 0;
    public float maxXp = 100;

    [SerializeField] private RawImage xpBarImage;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        currentXP = 0;
        maxXp = Player.Instance.ExperienceToNextLevel;
    }

    public void AddXP(float xp)
    {
        currentXP += xp;
        if (currentXP > maxXp)
        {
            currentXP = maxXp;
        }
        float fillAmount = currentXP / maxXp;
        StartCoroutine(ChangeXPBar(fillAmount));
    }

    private IEnumerator ChangeXPBar(float fillAmount)
    {
        float elapsedTime = 0;
        float originalFill = xpBar.sizeDelta.x;
        while (elapsedTime < fillSpeed)
        {
            elapsedTime += Time.deltaTime;
            xpBar.sizeDelta = new Vector2(Mathf.Lerp(originalFill, Screen.width * fillAmount, elapsedTime / fillSpeed), xpBar.sizeDelta.y);

            //change uv to make it look like the bar is moving
            xpBarImage.uvRect = new Rect(0, 0, xpBar.sizeDelta.x / xpBar.sizeDelta.y, 1);
            yield return null;
        }
    }

    public void ResetXP()
    {
        currentXP = 0;
        float fillAmount = currentXP / maxXp;
        StartCoroutine(ChangeXPBar(fillAmount));
    }

    public void SetMaxXP(float xp)
    {
        maxXp = xp;
    }

    void Update()
    {

        xpBarImage.uvRect = new Rect(xpBarImage.uvRect.x + Time.deltaTime * -0.2f, 0, xpBarImage.uvRect.width, 1);
    }

    public void SetLevel(int level)
    {
        levelText.text = level.ToString() + "LVL";
    }
}
