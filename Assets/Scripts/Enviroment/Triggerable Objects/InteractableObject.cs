using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableObject : TriggerableObject
{
    [Header("Interaction Settings")]
    public string interactionText = "Interact with this object";
    public BuffDebuffSystemBaseData effect; // Buff veya Debuff tanımlanır (ScriptableObject)

    [Header("Interaction Collider")]
    [SerializeField] private Vector3 gizmoSize = new Vector3(2f, 2f, 2f); // Collider boyutu
    [SerializeField] private Color gizmoColor = Color.blue; // Gizmo rengi

    private Collider interactionCollider;

    private void Awake()
    {
        // Collider'ı al ve boyutlarını uygula
        interactionCollider = GetComponent<Collider>();
        if (interactionCollider is BoxCollider boxCollider)
        {
            boxCollider.size = gizmoSize;
            boxCollider.isTrigger = true; // Etkileşim için trigger yap
        }
        else if (interactionCollider is SphereCollider sphereCollider)
        {
            sphereCollider.radius = gizmoSize.x / 2; // X boyutunu yarıçap olarak kullan
            sphereCollider.isTrigger = true; // Etkileşim için trigger yap
        }
    }

    public override string GetInteractionText()
    {
        return interactionText;
    }

    public override void ApplyInteractionEffect(float BuffEffectScaler , float DeBuffEffectScaler)
    {
        if (effect != null && CanInteract())
        {
            effect.SetHeroBuffEffectScaler(BuffEffectScaler);
            effect.SetHeroDeBuffEffectScaler(DeBuffEffectScaler);
            effect.ApplyBuffDeBuffSystem();
            isInteracted = true; // Etkileşim tamamlandı
            Debug.Log($"Effect Applied: {effect.effectName}");
            Debug.Log($"Effect Applied: {effect.description}");

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, gizmoSize); // BoxCollider için
    }
}