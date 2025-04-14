using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletWeapon : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f; // Merminin hızı
    [SerializeField] private GameObject explosionPrefab; // Patlama efekti prefab'i
    [SerializeField] private float explosionDuration = 2f; // Patlama efektinin süresi

    private Rigidbody rb;
    
    [SerializeField] private LayerMask affectedLayers;

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
    private void OnTriggerEnter(Collider other)
    {
        // Collision durumunu kontrol et (Layer'a göre)
        if ((affectedLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            // Eğer layer uyuyorsa, patlama efekti oluştur
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, explosionDuration);
            }

            Destroy(gameObject);
        }

        // Eğer bir Enemy objesine çarptıysa, hasar ver
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(1);
            Destroy(gameObject);
        }
    }

    
}