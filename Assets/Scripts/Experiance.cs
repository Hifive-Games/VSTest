using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiance : MonoBehaviour
{
    public int experience;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            ObjectPooler.Instance.ReturnObject(gameObject);

            GlobalGameEventManager.Instance.Notify("PlayerGetExperiance", experience);
        }
    }
}
