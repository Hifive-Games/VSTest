using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class DamageNumberUI : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float moveY = 50f;
    [SerializeField] private float duration = 0.6f;

    private RectTransform rectTransform;

    private CanvasGroup canvasGroup;

    private Sequence sequence;

    private static readonly Dictionary<DamageNumberType, Color> typeColors = new()
    {
        { DamageNumberType.Normal,   Color.white },
        { DamageNumberType.Critical, Color.red   },
        { DamageNumberType.Spell,    Color.blue  }
    };

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Play(int damage, Vector2 anchoredPos, Action<DamageNumberUI> onComplete, DamageNumberType type = DamageNumberType.Normal)
    {
        if (sequence != null&& sequence.IsActive())
        {
            sequence.Kill();
            sequence = null;
        }

        sequence = DOTween.Sequence();
        canvasGroup.alpha = 1f;


        if (damageText == null || onComplete == null)
        {
            Debug.LogError($"{name}: missing reference", this);
            onComplete?.Invoke(this);
            return;
        }

        damageText.color = typeColors[type];
        damageText.text = damage.ToString();
        rectTransform.anchoredPosition = anchoredPos;

        DOTween.Kill(rectTransform);
        sequence.Append(rectTransform
                    .DOAnchorPosY(anchoredPos.y + moveY, duration)
                    .SetEase(Ease.OutCubic))
           .Join(canvasGroup.DOFade(0f, duration))
           .OnStart(() => damageText.DOFade(1f, 0f))
           .OnComplete(() => onComplete(this));
    }
}