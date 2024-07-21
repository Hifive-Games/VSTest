using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiance : MonoBehaviour
{
    public int experience;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.AddExperience(experience);
            //play the sfx but start 0.2's in
            PlayerMagnet.Instance.audioSource.PlayOneShot(PlayerMagnet.Instance.expSfx);
            ObjectPooler.Instance.ReturnObject(gameObject, gameObject);
        }
    }
}
