using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifeTime;

    public void SetBullet(int damage, float speed, float lifeTime)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = lifeTime;
    }

    private void OnEnable()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Invoke("Die", lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemies enemy))
        {
            enemy.TakeDamage(damage);
            Die();
        }
    }

    private void Die()
    {
        ObjectPooler.Instance.ReturnObject(gameObject, gameObject);
    }
}
