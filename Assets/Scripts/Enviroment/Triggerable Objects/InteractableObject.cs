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

    private void CreateWorldTextUI()
    {
        GameObject canvasObj = new GameObject("InteractionTextCanvas");
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = textOffset;

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.scaleFactor = 10f;

        CanvasScaler scaler = canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 10f;

        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(2f, 1f);
        canvasRect.localScale = Vector3.one * 0.01f;

        GameObject textObj = new GameObject("InteractionText");
        textObj.transform.SetParent(canvasObj.transform, false);

        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = interactionText;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 24;
        text.color = Color.white;

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(200f, 50f);
        textRect.anchoredPosition = Vector2.zero;

        canvasTransform = canvasObj.transform;
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
            isInteracted = true;

            Debug.Log($"Effect Applied: {effect.effectName}");
            Debug.Log($"Effect Applied: {effect.description}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, gizmoSize);
    }
}
