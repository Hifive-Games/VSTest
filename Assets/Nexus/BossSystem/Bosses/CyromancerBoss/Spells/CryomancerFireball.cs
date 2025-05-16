using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryomancerFireball : MonoBehaviour
{
    [Header("Fireball Settings")]
    public float speed = 7.5f;
    public float damage = 10f;
    public float lifetime = 5f;

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody component not found on the fireball.");
            return;
        }
    }

    void Update()
    {
        // Move the fireball forward
        _rigidbody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);

        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the fireball hit the player
        if (other.TryGetComponent<TheHeroDamageManager>(out var damageManager))
        {
            // Apply damage to the player
            damageManager.TakeDamage(damage);
            Destroy();
        }
    }

    void Destroy()
    {
        ObjectPooler.Instance.ReturnObject(gameObject);
    }
}
