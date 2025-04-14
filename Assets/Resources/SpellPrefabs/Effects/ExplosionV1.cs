using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionV1 : MonoBehaviour
{
    private void OnEnable()
    {
        //radius = gameObject.GetComponent<ParticleSystem>().shape.angle / gameObject.GetComponent<ParticleSystem>().shape.radius;

        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
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
