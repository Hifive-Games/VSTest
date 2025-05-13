using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletWeapon : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f; // Merminin hızı
    [SerializeField] private GameObject explosionPrefab; // Patlama efekti prefab'i

    private Rigidbody rb;

    [SerializeField] private LayerMask affectedLayers;

    [SerializeField] private float bulletDamage = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnEnable()
    {
        rb.velocity = transform.forward * bulletSpeed;
        Invoke(nameof(ReturnBullet), 5f); // 5 saniye sonra yok ol
    }

    void OnDisable()
    {
        rb.velocity = Vector3.zero; // Mermi geri döndüğünde hızını sıfırla
        rb.angularVelocity = Vector3.zero; // Merminin açısal hızını sıfırla
    }
    private void OnTriggerEnter(Collider other)
    {   // Eğer bir Enemy objesine çarptıysa, hasar ver
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage((int)bulletDamage);
            ObjectPooler.Instance.ReturnObject(gameObject); // Mermiyi geri dön
        }
    }
    private void ReturnBullet()
    {
        // Mermiyi geri dön
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    public void SetBulletDamage(float damage)
    {
        bulletDamage = damage;
    }
}