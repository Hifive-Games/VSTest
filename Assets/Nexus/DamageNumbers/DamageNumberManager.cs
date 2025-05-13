using UnityEngine;
using System.Collections.Generic;

public enum DamageNumberType
{
    Normal,
    Critical,
    Spell
}

public class DamageNumberManager : MonoBehaviour
{
    public static DamageNumberManager Instance { get; private set; }

    [SerializeField] private DamageNumberUI damagePrefab;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private int initialPoolSize = 200;

    private readonly Queue<DamageNumberUI> pool = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        mainCamera ??= Camera.main;
        Preload(initialPoolSize);
    }

    private void Preload(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var ui = Instantiate(damagePrefab, canvasRect);
            ui.gameObject.SetActive(false);
            pool.Enqueue(ui);
        }
    }

    public void ShowDamage(int damage, Vector3 worldPosition, DamageNumberType type = DamageNumberType.Normal)
    {
        if (damagePrefab == null || canvasRect == null) return;
        if (pool.Count == 0) Preload(initialPoolSize);

        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenPos, mainCamera, out Vector2 localPos);

        var ui = pool.Dequeue();
        ui.gameObject.SetActive(true);
        ui.Play(damage, localPos, ReturnToPool, type);
    }

    private void ReturnToPool(DamageNumberUI ui)
    {
        ui.gameObject.SetActive(false);
        pool.Enqueue(ui);
    }
}