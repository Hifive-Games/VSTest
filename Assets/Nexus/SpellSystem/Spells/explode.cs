using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    void OnEnable()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(Disable());
    }

    void OnDisable()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }

    IEnumerator Disable()
    {
        yield return new WaitUntil(() => !gameObject.GetComponent<ParticleSystem>().isPlaying);
        ObjectPooler.Instance.ReturnObject(gameObject);
    }
}
