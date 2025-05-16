using UnityEngine;
using System.Collections;

public class ExperienceParticle : MonoBehaviour
{
    [SerializeField] public int experience = 1;
    [SerializeField] private float mergeDelay = 10f;
    [SerializeField] private float mergeRadius = 5f;
    [SerializeField] private LayerMask mergeLayer;
    private Transform _t;
    private Renderer _r;

    // Reusable buffer – size it to your max expected nearby count
    private static readonly Collider[] _buf = new Collider[32];

    private static readonly VisualSetting[] _settings =
    {
        new VisualSetting(10,    Color.yellow,  .75f),
        new VisualSetting(20,    Color.red, 1f),
        new VisualSetting(50,    Color.blue,    1.25f),
        new VisualSetting(100,   Color.magenta,   1.5f),
        new VisualSetting(int.MaxValue, Color.black, 2f)
    };

    private void Awake()
    {
        _t = transform;
        _r = GetComponentInChildren<Renderer>();
        //change emmision color and intensity to 1.5
        _r.material.SetColor("_EmissionColor", Color.green);
        _r.material.SetFloat("_EmissionIntensity", 1.5f);
        _t.localScale = Vector3.one * .5f;
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
        SFXManager.Instance.PlayAt(SFX.ExpShard);
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    private void MergeExp()
    {
        int initialExp = experience;

        int count = Physics.OverlapSphereNonAlloc(_t.position, mergeRadius, _buf, mergeLayer);

        if (count <= 1)
            return;

        int total = initialExp;
        for (int i = 0; i < count; i++)
        {
            if (_buf[i].TryGetComponent<ExperienceParticle>(out var p) && p != this)
            {
                total += p.experience;
                ObjectPooler.Instance.ReturnObject(p.gameObject);
            }
        }

        if (total == initialExp)
            return;

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
                _r.material.SetColor("_EmissionColor", s.Color);
                _r.material.SetFloat("_EmissionIntensity", 1.5f);
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
        _t.localScale = Vector3.one * .5f;
        _r.material.SetColor("_EmissionColor", Color.green);
        _r.material.SetFloat("_EmissionIntensity", 1.5f);
    }

    private readonly struct VisualSetting
    {
        public readonly int MaxExp;
        public readonly Color Color;
        public readonly float Scale;

        public VisualSetting(int maxExp, Color color, float scale)
        {
            MaxExp = maxExp;
            Color = color;
            Scale = scale;
        }
    }
}