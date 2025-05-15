using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ExperienceCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ExperienceParticle _))
        {
            //other.GetComponent<ExperienceParticle>().GetExperience();

            //tween the experience particle to the center of the collider but first must move away a bit then tween to the center
            Vector3 targetPosition = transform.position + new Vector3(0, .5f, 0);
            other.transform.DOMove(targetPosition, 0.3f).OnComplete(() =>
            {
                other.transform.DOMove(transform.position, 0.2f).OnComplete(() =>
                {
                    other.GetComponent<ExperienceParticle>().GetExperience();
                });
            });
        }
    }
}
