using UnityEngine;

public class BulletWeapon : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private LayerMask affectedLayers;

    [SerializeField] private float bulletDamage = 1;
    public float destroyTime = 2f;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (col == null)
            Debug.LogWarning("Bullet has no collider!");
    }

    private void OnEnable()
    {
        ResetBullet(); // Her aktifleştiğinde fizik reset
        Invoke(nameof(ReturnBullet), destroyTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void ResetBullet()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        rb.Sleep(); // Physics reset
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage((int)bulletDamage);
            ReturnBullet();
        }
    }

    private void ReturnBullet()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep(); // Bu kritik!
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    public void SetBulletDamage(float damage)
    {
        bulletDamage = damage;
    }

    public void Shoot()
    {
        rb.velocity = transform.forward * bulletSpeed;
        rb.WakeUp(); // Uyandır
    }
}
