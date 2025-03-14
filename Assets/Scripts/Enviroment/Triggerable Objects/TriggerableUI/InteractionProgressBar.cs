using UnityEngine;
using UnityEngine.UI;

public class InteractionProgressBar : MonoBehaviour
{
    public Slider progressBar;
    private float progress = 0f;
    private bool isInteracting = false;
    private float interactionTime = 0f;

    public void StartInteraction(float duration)
    {
        if (progressBar == null) return;

        interactionTime = duration;
        progress = 0f;
        isInteracting = true;
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0f;
    }

    public void CancelInteraction()
    {
        if (progressBar == null) return;

        isInteracting = false;
        progressBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isInteracting)
        {
            progress += Time.deltaTime / interactionTime;
            progressBar.value = progress;

            if (progress >= 1f)
            {
                isInteracting = false;
                progressBar.gameObject.SetActive(false);

                // Karakterin etkileşim tamamladığı yerde gerekli işlem yapılabilir.
                TheHeroInteraction.Instance.CompleteInteraction();
            }
        }
    }
}