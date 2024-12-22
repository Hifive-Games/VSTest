using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using NaughtyAttributes;
using UnityEngine;

public class TheHeroHealth : MonoBehaviour
{
    
    private float currentHealth;
    private float maxHealth;

    private float healthRegenAmount = 1f; // Sağlık yenileme miktarı
    private float healthRegenRate = 1f; // Sağlık yenileme süresi (saniye)
    
    private float timer = 0f; // Sağlık yenileme için zamanlayıcı


    // Can 0 olduğunda tetiklenecek Action
    public event Action OnHealthDepleted;
    
    private void FixedUpdate()
    {
        RegenerateHealthOverTime();
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnHealthDepleted?.Invoke(); // Can 0 olduğunda Action tetiklenir
        }
    }


    private void RegenerateHealthOverTime()
    {
        if (currentHealth >= maxHealth)
        {
            return; // Maksimum sağlığa ulaşıldıysa işlem yapma
        }

        timer += Time.deltaTime;
        if (timer >= healthRegenRate)
        {
            timer = 0f;
            AddHealth(healthRegenAmount);
        }
    }
    
    
    public void SetMaximumHealth(float health)
    {
        currentHealth = maxHealth = health;
    }
    // bi yerde lazım olur belki
    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
    }
    public void SetHealthRegenAmount(float health)
    {
        healthRegenAmount = Mathf.Clamp(health, 0, maxHealth);
    }

    public void AddHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);
    }
    public float GetHealth()
    {
        return currentHealth;
    }
    public void SetHealthRegenRate(float newRate)
    {
        healthRegenRate = Mathf.Clamp(newRate, 0, maxHealth);
    }
    
    [Button()]
    public void ShowInfo()
    {
        Debug.LogError(" Current health : "+currentHealth);
        Debug.LogError(" Max health : "+maxHealth);
        Debug.LogError(" HealthRegenAmount : "+healthRegenAmount);
        Debug.LogError(" HealthRegenRate : "+healthRegenRate);
    }

    [Button()]
    public void TestTakeDamage()
    {
        currentHealth -= 30;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnHealthDepleted?.Invoke(); // Can 0 olduğunda Action tetiklenir
        }
    }
}

