using System.Collections;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn; // Spawn edilecek obje
    [SerializeField] private float spawnInterval = 1f; // Obje spawn süresi

    private Coroutine spawnRoutine;

    private void Start()
    {
        StartSpawning();
    }
    
    public void StartSpawning()
    {
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        spawnRoutine = StartCoroutine(SpawnObjects());
    }
    
    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }
    
    public void SetSpawnInterval(float newInterval)
    {
        spawnInterval = Mathf.Max(0.1f, newInterval); // Spawn süresi sıfır veya negatif olmamalı
        StartSpawning(); // Yeni süreye göre spawn işlemini yeniden başlat
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    private void SpawnObject()
    {
        if (objectToSpawn != null )
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("ObjectToSpawn veya SpawnPoint atanmadı!");
        }
    }
}