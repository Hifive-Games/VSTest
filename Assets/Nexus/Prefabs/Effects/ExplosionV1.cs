using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionV1 : MonoBehaviour
{
    //this is a pool object we call it when a spell projectile hits something so we can reuse it and use onenable and ondisable. when its enabled it will get all objects with the component enemy and deal damage to them

    public int damage = 10;
    public float radius = 5f;

    private void OnEnable()
    {
        radius = gameObject.GetComponent<ParticleSystem>().shape.angle / gameObject.GetComponent<ParticleSystem>().shape.radius;

        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
        StartCoroutine(DestroyAfterLifetime());
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitUntil(() => gameObject.GetComponent<ParticleSystem>().isStopped);
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
