using UnityEngine;
using System.Collections;

public class ExperienceParticle : MonoBehaviour
{
    [SerializeField] public int experience = 1;
    [SerializeField] private float mergeDelay = 30f;
    [SerializeField] private float mergeRadius = 5f;
    [SerializeField] private LayerMask mergeLayer;
    private Transform _t;
    private Renderer _r;

    // Reusable buffer – size it to your max expected nearby count
    private static readonly Collider[] _buf = new Collider[32];

    private static readonly VisualSetting[] _settings =
    {
        new VisualSetting(10,    Color.green,  0.5f),
        new VisualSetting(20,    Color.yellow, 1f),
        new VisualSetting(50,    Color.red,    1.5f),
        new VisualSetting(100,   Color.blue,   2f),
        new VisualSetting(int.MaxValue, Color.white, 1f)
    };

    private void Awake()
    {
        _t = transform;
        _r = GetComponentInChildren<Renderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(MergeAfterDelay());
    }

    private IEnumerator MergeAfterDelay()
    {
        yield return new WaitForSeconds(mergeDelay);
        MergeExp();
    }

    public void GetExperience()
    {
        GameEvents.OnExperienceGathered?.Invoke(experience);
        ObjectPooler.Instance.ReturnObject(gameObject);
        SFXManager.Instance.PlayAt(SFX.ExpShard);
    }

    private void MergeExp()
    {
        int count = Physics.OverlapSphereNonAlloc(_t.position, mergeRadius, _buf, mergeLayer);
        int total = experience;

        for (int i = 0; i < count; i++)
        {
            if (_buf[i].TryGetComponent<ExperienceParticle>(out var p) && p != this)
            {
                total += p.experience;
                ObjectPooler.Instance.ReturnObject(p.gameObject);
            }
        }

        experience = total;
        ApplyVisuals();
    }

    private void ApplyVisuals()
    {
        foreach (var s in _settings)
        {
            if (experience <= s.MaxExp)
            {
                _t.localScale = Vector3.one * s.Scale;
                _r.material.color = s.Color;
                return;
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetVisual();
    }

    private void ResetVisual()
    {
        _t.localScale = Vector3.one;
        _r.material.color = Color.white;
    }

    private readonly struct VisualSetting
    {
        public readonly int MaxExp;
        public readonly Color Color;
        public readonly float Scale;

        public VisualSetting(int maxExp, Color color, float scale)
        {
            MaxExp = maxExp;
            Color  = color;
            Scale  = scale;
        }
    }
}