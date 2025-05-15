using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSh : MonoBehaviour
{
    public float speed = 1f;
    //if the game object is not active, it will not rotate
    private void OnEnable()
    {
        StartCoroutine(Rotate());
    }
    private IEnumerator Rotate()
    {
        while (gameObject.activeSelf)
        {
            transform.Rotate(Vector3.up, 360 * Time.deltaTime * speed);
            yield return null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
