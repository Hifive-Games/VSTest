using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class LightningOrbRenderer : MonoBehaviour
{
    #region Enums & Constants
    public enum TriggerMode { Band, Timed, Both }
    private const int DEFAULT_MAX_SEGMENTS = 11;
    #endregion

    #region Inspector Settings
    [Header("Trigger Settings")]
    public TriggerMode triggerMode = TriggerMode.Band;
    public float sparkInterval = 0.2f;

    [Header("Spark Settings")]
    public int maxSparksInPool = 20;
    public float sparkDuration = 0.05f;
    public float sparkRadius = 3f;
    public Material sparkMaterial;

    [Header("Core Orb")]
    public float orbRadius = 0.2f;
    #endregion

    #region Private Fields
    private float sparkTimer;
    private Vector3[] lightningPoints = new Vector3[DEFAULT_MAX_SEGMENTS];

    // Pool of line‐renderers (sparks)
    private List<LineRenderer> sparkPool = new List<LineRenderer>();
    private int nextSparkIndex;

    // Active sparks and their fade data
    private struct SparkData
    {
        public LineRenderer lr;
        public Light pointLight;
        public float elapsed;
        public float duration;
        public float initialWidth;
        public float initialIntensity;
    }
    private List<SparkData> activeSparks = new List<SparkData>();

    // Band trigger flags
    private bool[] bandTriggered = new bool[8];

    // Core orb visuals
    private GameObject coreVisual;
    private Material orbMaterial;
    #endregion

    #region Unity Event Hooks
    private void OnEnable() => AudioSpectrum.OnBandTrigger += OnBandTrigger;
    private void OnDisable() => AudioSpectrum.OnBandTrigger -= OnBandTrigger;
    private void OnDestroy() => AudioSpectrum.OnBandTrigger -= OnBandTrigger;
    #endregion

    #region Initialization
    private void Start()
    {
        orbMaterial = sparkMaterial;
        CreateSparkPool();
        CreateCoreOrb();
    }

    private void CreateSparkPool()
    {
        for (int i = 0; i < maxSparksInPool; i++)
        {
            var go = new GameObject($"Spark_{i}");
            go.transform.SetParent(transform);

            // LineRenderer
            var lr = go.AddComponent<LineRenderer>();
            lr.material = sparkMaterial;
            lr.positionCount = 2;
            lr.useWorldSpace = true;
            lr.enabled = false;
            lr.numCornerVertices = 5;
            lr.numCapVertices = 5;
            lr.textureMode = LineTextureMode.Tile;

            // Point Light
            var lt = go.AddComponent<Light>();
            lt.type = LightType.Point;
            lt.range = sparkRadius * 1.5f;
            lt.intensity = 0f;
            lt.color = sparkMaterial.color;
            lt.enabled = false;

            sparkPool.Add(lr);
        }
    }

    private void CreateCoreOrb()
    {
        coreVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        coreVisual.transform.SetParent(transform);
        coreVisual.transform.localPosition = Vector3.zero;
        coreVisual.transform.localScale = Vector3.one * orbRadius;
        Destroy(coreVisual.GetComponent<Collider>());
        coreVisual.GetComponent<Renderer>().material = orbMaterial;
    }
    #endregion

    #region Update Loop
    private void Update()
    {
        HandleTimedTrigger();
        HandleBandTrigger();
        UpdateActiveSparks(Time.deltaTime);
    }

    private void HandleTimedTrigger()
    {
        if (triggerMode == TriggerMode.Timed || triggerMode == TriggerMode.Both)
        {
            sparkTimer += Time.deltaTime;
            if (sparkTimer >= sparkInterval)
            {
                EmitRandomSpark();
                sparkTimer = 0f;
            }
        }
    }

    private void HandleBandTrigger()
    {
        if (triggerMode == TriggerMode.Band || triggerMode == TriggerMode.Both)
        {
            for (int b = 0; b < bandTriggered.Length; b++)
            {
                if (bandTriggered[b])
                {
                    EmitRandomSpark();
                    bandTriggered[b] = false;
                }
            }
        }
    }

    private void UpdateActiveSparks(float delta)
    {
        for (int i = activeSparks.Count - 1; i >= 0; i--)
        {
            var data = activeSparks[i];
            data.elapsed += delta;
            float t = data.elapsed / data.duration;

            if (t >= 1f)
            {
                data.lr.enabled = false;
                data.pointLight.enabled = false;
                activeSparks.RemoveAt(i);
            }
            else
            {
                FadeSpark(data, t);
                activeSparks[i] = data;
            }
        }
    }

    private void FadeSpark(SparkData sd, float t)
    {
        // Line fade
        float alpha = Mathf.Lerp(1f, 0f, t);
        var sc = sd.lr.startColor; sc.a = alpha;
        sd.lr.startColor = sc;
        sd.lr.endColor = new Color(sc.r, sc.g, sc.b, 0f);
        sd.lr.widthMultiplier = Mathf.Lerp(sd.initialWidth, 0f, t);

        // Light fade
        sd.pointLight.intensity = Mathf.Lerp(sd.initialIntensity, 0f, t);
    }
    #endregion

    #region Event Handlers
    private void OnBandTrigger(int band)
    {
        if (band >= 0 && band < bandTriggered.Length)
            bandTriggered[band] = true;
    }
    #endregion

    #region Spark Emission
    public void EmitRandomSpark()
    {
        Vector3 origin = transform.position;
        Vector2 circle = Random.insideUnitCircle * sparkRadius;
        Vector3 target = origin + new Vector3(circle.x,
                                              Random.Range(-sparkRadius * .2f, sparkRadius * .2f),
                                              circle.y);

        int segments = Random.Range(6, DEFAULT_MAX_SEGMENTS);
        GenerateLightningArc(origin, target, segments, sparkRadius * 0.5f, lightningPoints);

        Color tint = Color.Lerp(Color.white, Color.cyan, Random.value);
        EmitSpark(lightningPoints, segments, sparkDuration, tint);
    }

    public void EmitSpark(Vector3[] points, int count, float duration, Color? tint = null)
    {
        if (sparkPool.Count == 0) return;

        var lr = sparkPool[nextSparkIndex];
        var lt = lr.GetComponent<Light>();
        nextSparkIndex = (nextSparkIndex + 1) % sparkPool.Count;

        // Configure LineRenderer
        lr.positionCount = count;
        for (int i = 0; i < count; i++)
            lr.SetPosition(i, points[i]);

        Color color = tint ?? sparkMaterial.color;
        lr.startColor = color;
        lr.endColor = new Color(color.r, color.g, color.b, 0f);
        float startWidth = Random.Range(0.05f, 0.1f);
        lr.widthMultiplier = startWidth;
        lr.enabled = true;

        // Configure Light
        lt.transform.position = Vector3.Lerp(points[0], points[count - 1], 0.5f);
        lt.color = color;
        float startIntensity = 2f;
        lt.intensity = startIntensity;
        lt.enabled = true;

        // Track for fading
        activeSparks.Add(new SparkData
        {
            lr = lr,
            pointLight = lt,
            elapsed = 0f,
            duration = duration,
            initialWidth = startWidth,
            initialIntensity = startIntensity
        });
    }
    #endregion

    #region Lightning Generator
    private void GenerateLightningArc(
        Vector3 start, Vector3 end,
        int segments, float chaos, Vector3[] buffer)
    {
        buffer[0] = start;
        Vector3 direction = (end - start).normalized;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up);

        for (int i = 1; i < segments - 1; i++)
        {
            float t = (float)i / (segments - 1);
            Vector3 point = Vector3.Lerp(start, end, t);
            float scale = 1f - Mathf.Abs(2f * t - 1f);
            float offset = Random.Range(-chaos, chaos) * scale;
            point += perpendicular * offset + Random.insideUnitSphere * (chaos * 0.1f);
            buffer[i] = point;
        }

        buffer[segments - 1] = end;
    }
    #endregion
}