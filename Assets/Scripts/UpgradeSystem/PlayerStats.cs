using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviourSingleton<PlayerStats>
{
    public int currentHealth;
    public int maxHealth;
    public float armor;
    public float moveSpeed;
    public bool isInvincible = false;

    private void Start()
    {
        InitializePlayer();
    }

    public void InitializePlayer(HeroBaseData heroBaseData = null)
    {
        if (heroBaseData != null)
        {
            /*
            maxHealth = playerSO.maxHealth;
            currentHealth = maxHealth;
            armor = playerSO.armor;
            moveSpeed = playerSO.moveSpeed;
            */
        }
        else
        {
            maxHealth = 100;
            currentHealth = maxHealth;
            armor = 0;
            moveSpeed = 5;
        }

        StartCoroutine(BecomeInvincible());
    }

     public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(BecomeInvincible(0.2f));
    }

    
    public void Die()
    {
        Debug.Log("Player died!");
        GameManager.Instance.GameOver();
    }

    private IEnumerator BecomeInvincible(float duration = 5f)
    {
        MeshRenderer _renderer = GetComponent<MeshRenderer>();
        Color _orjMat = _renderer.material.color;
        _renderer.material.color = Color.red;
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
        _renderer.material.color = _orjMat;
        
    }
}
