using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletWeapon : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f; // Merminin hızı
    [SerializeField] private GameObject explosionPrefab; // Patlama efekti prefab'i
    [SerializeField] private float explosionDuration = 2f; // Patlama efektinin süresi
    [SerializeField] private float selfDestructTime = 5f; // Çarpmazsa kendini yok etme süresi

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Yer çekiminden etkilenmemesi için gravity'yi devre dışı bırak
        rb.useGravity = false;
    }

    private void Start()
    {
        // Mermiyi ileriye doğru sürekli hareket ettir
        rb.velocity = transform.forward * bulletSpeed;

        // Çarpmazsa belirli bir süre sonra kendini yok et
        Invoke(nameof(DestroyWithoutExplosion), selfDestructTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Çarptığında patlama efekti oluştur
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Patlama efektini belirli bir süre sonra yok et
            Destroy(explosion, explosionDuration);
        }

        // Kendini yok et
        Destroy(gameObject);
    }

    private void DestroyWithoutExplosion()
    {
        // Çarpma olmadan kendini yok et
        Destroy(gameObject);
    }
}