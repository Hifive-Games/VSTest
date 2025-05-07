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

        // Merminin belli bir süre sonra yok olmasını sağla
        Invoke(nameof(ReturnBullet), 5f); // 5 saniye sonra yok ol
    }
    private void OnTriggerEnter(Collider other)
    {        // Eğer bir Enemy objesine çarptıysa, hasar ver
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(1);
            ObjectPooler.Instance.ReturnObject(gameObject); // Mermiyi geri dön
        }
    }

    private void ReturnBullet()
    {
        // Mermiyi geri dön
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

}