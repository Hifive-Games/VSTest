using UnityEngine;

public abstract class TriggerableObject : MonoBehaviour
{
    public float interactionDuration = 5.0f; // Varsayılan süre
    protected bool isInteracted = false;    // Etkileşim tamamlandı mı?

    public abstract string GetInteractionText(); // Ekranda gösterilecek yazı
    public abstract void ApplyInteractionEffect(float BuffEffectScaler, float DeBuffEffectScaler); // Etkileşim tamamlandığında yapılacak

    public bool CanInteract()
    {
        return !isInteracted; // Sadece henüz etkileşime geçilmediyse true döner
    }
}