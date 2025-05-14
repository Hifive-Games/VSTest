using UnityEditor.SceneManagement;
using UnityEngine;

public class BulletWeapon : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f; // Merminin hızı
    [SerializeField] private GameObject explosionPrefab; // Patlama efekti prefab'i

    [SerializeField] private LayerMask affectedLayers;

    [SerializeField] private float bulletDamage = 1;

    public float destroyTime = 5f; // Merminin yok olma süresi

    private void OnTriggerEnter(Collider other)
    {   // Eğer bir Enemy objesine çarptıysa, hasar ver
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage((int)bulletDamage);
            // Mermi geri döndüğünde Rigidbody bileşenini kaldır
            if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Destroy(rb);
            }
            ObjectPooler.Instance.ReturnObject(gameObject);
            CancelInvoke("ReturnBullet");
        }
    }
    private void ReturnBullet()
    {
        // Mermi geri döndüğünde Rigidbody bileşenini kaldır
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Destroy(rb);
        }
        // Mermiyi geri dön
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    public void SetBulletDamage(float damage)
    {
        Rigidbody rb;
        gameObject.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Merminin yer çekiminden etkilenmemesi için

        rb.velocity = transform.forward * bulletSpeed;
        bulletDamage = damage;

        Invoke("ReturnBullet", destroyTime);
    }
}