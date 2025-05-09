using UnityEngine;
using UnityEngine.UI;

public class InteractionProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressFillImage;
    [SerializeField] private Image progressBackgroundImage;
    [SerializeField] private TMPro.TextMeshProUGUI progressText;
    private float progress = 0f;
    private bool isInteracting = false;
    private float interactionTime = 0f;

    public void StartInteraction(float duration)
    {
        if (progressBackgroundImage == null) return;

        interactionTime = duration;
        progress = 0f;
        isInteracting = true;
        progressBackgroundImage.gameObject.SetActive(true);
        progressFillImage.fillAmount = 0f;
        progressText.text = "0%";
    }

    public void CancelInteraction()
    {
        if (progressBackgroundImage == null) return;

        isInteracting = false;
        progressBackgroundImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isInteracting)
        {
            progress += Time.deltaTime / interactionTime;
            progressFillImage.fillAmount = progress;
            progressText.text = $"{(int)(progress * 100)}%";

            if (progress >= 1f)
            {
                isInteracting = false;
                progressBackgroundImage.gameObject.SetActive(false);
                TheHeroInteraction.Instance.CompleteInteraction();
            }
        }
    }
}