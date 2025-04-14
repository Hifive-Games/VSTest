using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out _))
        {
            SpellSelectionManager.Instance.ChestOpened(); // Sandık açıldığında büyü seçim panelini aç

            ObjectPooler.Instance.ReturnObject(gameObject); // Sandığı geri gönder
        }
    }
}
