using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyDed : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<BulletWeapon>())
        Destroy(gameObject);
    }
}
