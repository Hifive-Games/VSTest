using UnityEngine;
using System.Collections.Generic;

public class LightningOrb : MonoBehaviour
{
    [Header("Orb Settings")]
    public float sparkInterval = 0.2f;
    public float sparkRadius = 3f;
    public Material sparkMaterial;
    public int sparkCount = 20;
    public float sparkDuration = 0.05f;

    private float timer;
    private List<LineRenderer> sparks = new List<LineRenderer>();
    private int currentSparkIndex = 0;

    void Start()
    {
        // Pre-create spark lines (object pooling)
        for (int i = 0; i < sparkCount; i++)
        {
            GameObject obj = new GameObject("Spark_" + i);
            obj.transform.parent = transform;
            LineRenderer lr = obj.AddComponent<LineRenderer>();
            lr.material = sparkMaterial;
            lr.widthMultiplier = 0.05f;
            lr.positionCount = 2;
            lr.enabled = false;
            sparks.Add(lr);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= sparkInterval)
        {
            timer = 0f;
            EmitSpark();
        }
    }

    void EmitSpark()
    {
        int segments = Random.Range(3, 6); // 3 to 5 points
        Vector3 start = transform.position + Random.insideUnitSphere * 0.2f;
        Vector3 end = transform.position + Random.insideUnitSphere * sparkRadius;

        LineRenderer lr = sparks[currentSparkIndex];
        currentSparkIndex = (currentSparkIndex + 1) % sparks.Count;

        lr.positionCount = segments;
        Vector3 direction = (end - start).normalized;
        float totalLength = Vector3.Distance(start, end);

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);
            Vector3 point = Vector3.Lerp(start, end, t);

            // Add random offset to simulate chaos (except start & end)
            if (i != 0 && i != segments - 1)
            {
                Vector3 randomOffset = Random.insideUnitSphere * 0.3f;
                point += randomOffset;
            }

            lr.SetPosition(i, point);
        }

        StartCoroutine(FlashSpark(lr));
    }


    System.Collections.IEnumerator FlashSpark(LineRenderer lr)
    {
        lr.enabled = true;
        yield return new WaitForSeconds(sparkDuration);
        lr.enabled = false;
    }
}
