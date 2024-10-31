using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Enemies
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
    private void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.LookAt(player.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
        }
    }
}
