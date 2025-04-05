using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceParticle : MonoBehaviour
{
    public int experience=1;

    public void GetExperience()
    {
        GameEvents.OnExperienceGathered.Invoke(experience);

        ObjectPooler.Instance.ReturnObject(gameObject);
    }
}
