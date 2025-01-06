using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    //this will be the enemy class, it will inherit from the Enemies class and if the enemy gameobject is active it will move towards the player

    public Material material;

    private void Awake()
    {
        material = transform.GetComponentInChildren<MeshRenderer>().material;
    
    }
    private new void OnEnable()
    {
        material.color = Random.ColorHSV();
        base.OnEnable();
    }

    private new void  OnDisable()
    {
        base.OnDisable();
    }
}
