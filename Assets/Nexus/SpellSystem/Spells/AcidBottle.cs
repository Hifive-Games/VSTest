using System.Collections;
using UnityEngine;

public class AcidBottle : Spell
{
    [SerializeField] private GameObject acidAreaPrefab; // Acid Area prefab to spawn upon impact
    [SerializeField] private float arcHeight = 5f;     // Height of the projectile's arc
    [SerializeField] private float travelTime = 2f;    // Time it takes for the projectile to reach its target

    public override void OnEnable()
    {
        StartCoroutine(ThrowBottle());
    }

    public override void OnDisable()
    {
        StopAllCoroutines();
    }

    public override void Seek(Transform target = null)
    {
        // No need to seek a target for this spell
    }

    public override void Release()
    {
    }

    private IEnumerator ThrowBottle()
    {
        // Generate a random target point within range
        Vector3 randomPoint = GetRandomPointInRange();
        Vector3 startPoint = transform.position;

        // Calculate travel time
        float _travelTime = travelTime;
        float elapsedTime = 0;

        while (elapsedTime < _travelTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _travelTime;

            // Calculate position along a parabolic arc
            Vector3 currentPos = Vector3.Lerp(startPoint, randomPoint, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

            transform.position = currentPos;
            yield return null;
        }

        // Spawn acid area at the target point
        SpawnAcidArea(randomPoint);
        ObjectPooler.Instance.ReturnObject(gameObject); // Return bottle to the object pool
    }

    private Vector3 GetRandomPointInRange()
    {
        Vector2 randomCircle = Random.insideUnitCircle * range;
        Vector3 randomPoint = new Vector3(randomCircle.x, 0, randomCircle.y);
        randomPoint += transform.position;

        // Ensure it's on the ground (Y = 0)
        randomPoint.y = 0;

        return randomPoint;
    }

    private void SpawnAcidArea(Vector3 position)
    {
        GameObject acidArea = ObjectPooler.Instance.SpawnFromPool(acidAreaPrefab, position, Quaternion.identity);
        acidArea.transform.position = new Vector3(acidArea.transform.position.x, 0.1f, acidArea.transform.position.z); // Offset Y position
        acidArea.transform.rotation = Quaternion.Euler(90, 0, 0); // Rotate to face upwards
        acidArea.GetComponent<AcidAreaEffect>().Initialize(damage, radius, tickInterval, duration, Caster);
    }
}
