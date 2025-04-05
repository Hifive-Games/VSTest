using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ExperienceParticle _))
        {
            other.GetComponent<ExperienceParticle>().GetExperience();
        }
    }
}
