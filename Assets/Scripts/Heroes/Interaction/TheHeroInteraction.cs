using System;
using UnityEngine;

public class TheHeroInteraction : MonoBehaviourSingleton<TheHeroInteraction>
{
    private TriggerableObject currentObject;
    private InteractionProgressBar progressBar; // Progress bar referansı

    private float BuffEffectScaler;
    private float DeBuffEffectScaler;

    private void Awake()
    {
        MainGamePanelManager mainGamePanelManager = FindObjectOfType<MainGamePanelManager>();
        progressBar = mainGamePanelManager.GetProgressBar();
    }

    private void OnTriggerEnter(Collider other)
    {
        var triggerable = other.GetComponent<TriggerableObject>();
        if (triggerable != null && triggerable.CanInteract())
        {
            currentObject = triggerable;
            Debug.LogError(triggerable.GetInteractionText());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentObject != null && other.GetComponent<TriggerableObject>() == currentObject)
        {
            CancelInteraction();
            currentObject = null;
        }
    }

    private void Update()
    {
        if (currentObject != null && Input.GetKeyDown(KeyCode.E))
        {
            StartInteraction();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            CancelInteraction();
        }
    }

    private void StartInteraction()
    {
        if (currentObject != null && progressBar != null)
        {
            progressBar.StartInteraction(currentObject.interactionDuration);
        }
    }

    private void CancelInteraction()
    {
        if (progressBar != null)
        {
            progressBar.CancelInteraction();
        }
    }

    public void CompleteInteraction()
    {
        if (currentObject != null)
        {
            currentObject.ApplyInteractionEffect(BuffEffectScaler,DeBuffEffectScaler);
            currentObject = null;
        }
    }

    public void SetBuffEffectScaler(float value)
    {
        BuffEffectScaler = Mathf.Max(0, value);
    }

    public void AddBuffEffectScaler(float value)
    {
        BuffEffectScaler = Mathf.Max(0, BuffEffectScaler + value);
    }

    public void ReduceBuffEffectScaler(float value)
    {
        BuffEffectScaler = Mathf.Max(0, BuffEffectScaler - value);
    }

    public void SetDeBuffEffectScaler(float value)
    {
        DeBuffEffectScaler = Mathf.Max(0, value);
    }

    public void AddDeBuffEffectScaler(float value)
    {
        DeBuffEffectScaler = Mathf.Max(0, DeBuffEffectScaler + value);
    }

    public void ReduceDeBuffEffectScaler(float value)
    {
        DeBuffEffectScaler = Mathf.Max(0, DeBuffEffectScaler - value);
    }


}