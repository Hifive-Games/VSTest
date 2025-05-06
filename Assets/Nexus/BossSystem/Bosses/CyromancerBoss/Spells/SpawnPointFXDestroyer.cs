using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointFXDestroyer : MonoBehaviour
{
    public float destroyTime = 1f; // time in seconds before the object is destroyed

    public void SetDestroyTime(float time)
    {
        destroyTime = time;
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
