using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletWeapon : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f; // Merminin hızı
    [SerializeField] private GameObject explosionPrefab; // Patlama efekti prefab'i
    [SerializeField] private float explosionDuration = 2f; // Patlama efektinin süresi

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Start()
    {
        // Mermiyi ileriye doğru sürekli hareket ettir
        rb.velocity = transform.forward * bulletSpeed;
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

        Destroy(gameObject);
    }

    
}