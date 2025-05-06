using UnityEngine;
using TMPro;
using UnityEngine.UI;


[RequireComponent(typeof(Collider))]
public class InteractableObject : TriggerableObject
{
    [Header("Interaction Settings")]
    public string interactionText = "Interact with this object";
    public BuffDebuffSystemBaseData effect;

    [Header("Interaction Collider")]
    [SerializeField] private Vector3 gizmoSize = new Vector3(2f, 2f, 2f);
    [SerializeField] private Color gizmoColor = Color.blue;

    [Header("UI Settings")]
    [SerializeField] private Vector3 textOffset = new Vector3(0f, 1.5f, 0f);
    
    private Collider interactionCollider;
    private Transform canvasTransform;

    [SerializeField] private GameObject canvasObject; // sahnede bulunan InteractionTextCanvas objesi
    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        interactionCollider = GetComponent<Collider>();
        if (interactionCollider is BoxCollider boxCollider)
        {
            boxCollider.size = gizmoSize;
            boxCollider.isTrigger = true;
        }
        else if (interactionCollider is SphereCollider sphereCollider)
        {
            sphereCollider.radius = gizmoSize.x / 2;
            sphereCollider.isTrigger = true;
        }

        SetLayer();
        CreateWorldTextUI();
    }

    private void SetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Colliders");
    }

    public void CreateWorldTextUI()
    {
        if (canvasObject == null)
        {
            Debug.LogError("Canvas Object is not assigned.");
            return;
        }

        // Canvas objesini bu objenin altına yerleştir ve pozisyonla
        canvasTransform = canvasObject.transform;
        canvasTransform.SetParent(transform);
        canvasTransform.localPosition = textOffset;

        // Canvas ayarları
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
        }

        // Text bileşenini bul
        textComponent = canvasObject.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in children.");
            return;
        }

        // Text ayarları
        textComponent.text = interactionText;
        textComponent.enableAutoSizing = true;
        textComponent.fontSizeMin = 14;
        textComponent.fontSizeMax = 36;
        textComponent.color = Color.white;
        textComponent.outlineWidth = 0.08f;
        textComponent.outlineColor = Color.black;
    }

    public void UpdateInteractableObjectText(string newText)
    {
        interactionText = newText;
        if (textComponent != null)
        {
            textComponent.text = newText;
        }
    }

    private void LateUpdate()
    {
        if (canvasTransform != null && Camera.main != null)
        {
            canvasTransform.rotation = Quaternion.LookRotation(canvasTransform.position - Camera.main.transform.position);
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
            
            SetInteracted();

            Debug.Log($"Effect Applied: {effect.effectName}");
            Debug.Log($"Effect Applied: {effect.description}");
            UpdateInteractableObjectText(effect.GetBuffDebuffText());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, gizmoSize);
    }
}
