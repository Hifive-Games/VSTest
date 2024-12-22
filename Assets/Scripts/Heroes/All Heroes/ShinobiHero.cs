using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class ShinobiHero : TheHero
{
    [SerializeField] private GameObject objectToSpawn; // Spawn edilecek obje
    [SerializeField] private Transform spawnParent; // Spawn noktası
    [SerializeField] private float spawnRate = 1f; // Saniyede kaç kez spawn yapılacak
    [SerializeField] private float selfDestructTime = 0.5f; // Obje yok olma süresi
    [SerializeField] private int shurikenCount = 1; // Üretilecek obje sayısı
    [SerializeField] private Vector3 defaultScale = new Vector3(5f, 5f, 5f); // Obje scale değeri

    private float spawnTimer;

    private void Start()
    {
        // Başlangıç scale ayarı
        if (objectToSpawn != null)
        {
            objectToSpawn.transform.localScale = defaultScale;
        }
    }

    private void FixedUpdate()
    {
        CheckSpawnTime();
    }

    void CheckSpawnTime()
    {
        if (spawnRate > 0)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= 1f / spawnRate)
            {
                spawnTimer = 0f;
                SpawnObjects();
            }
        }
    }

    // Obje spawn etme işlemi
    private void SpawnObjects()
    {
        if (objectToSpawn != null && spawnParent != null)
        {
            for (int i = 1; i <= shurikenCount; i++)
            {
                Vector3 offset = GetSpawnOffset(i);
                GameObject temp = Instantiate(objectToSpawn, spawnParent.position + offset, Quaternion.LookRotation(offset != Vector3.zero ? offset : spawnParent.forward));
                temp.transform.localScale = defaultScale; // Scale ayarı
                StartCoroutine(DestroyAfterDelay(temp, selfDestructTime));
            }
        }
        else if (objectToSpawn == null)
        {
            Debug.LogWarning("ObjectToSpawn atanmadı!");
        }
        else
        {
            Debug.LogWarning("SpawnPoint atanmadı!");
        }
    }

    // Spawn pozisyonlarını belirle
    private Vector3 GetSpawnOffset(int index)
    {
        if (shurikenCount == 1 || index == 1)
        {
            return spawnParent.forward * 2f; // İlk şuriken ileri doğru atılır.
        }

        float angleStep = 360f / shurikenCount; // Her şuriken arasındaki açı
        float angle = angleStep * index; // İlgili şurikenin açısı

        // İlk şurikenin yönüne göre diğerlerini açılı olarak yerleştir
        Vector3 baseDirection = spawnParent.forward;
        Quaternion rotation = Quaternion.Euler(0, angle - angleStep, 0); // İlk şuriken baz alınır
        Vector3 offsetDirection = rotation * baseDirection;

        return offsetDirection * 2f; // Mesafe çarpanı
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(obj);
    }

    // Burası TheHero'dan Override edilen fonksiyonlar
    public override void SetAttackSpeed(float newRate)
    {
        spawnRate = Mathf.Max(0.1f, newRate);
    }

    public override void SetAttackRange(float newRate)
    {
        selfDestructTime = Mathf.Max(0.1f, newRate);
    }

    public override void SetAttackSize(float newRate)
    {
        defaultScale = new Vector3(newRate, newRate, newRate);
    }

    public override void SetAttackAmount(float newRate)
    {
        shurikenCount = (int)newRate;
    }
}