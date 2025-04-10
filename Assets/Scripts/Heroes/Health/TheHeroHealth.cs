using UnityEngine;
using System;
using NaughtyAttributes;

public class TheHeroHealth : MonoBehaviour
{
    /*
     * MAXİMUM CAN VE REDUCE MAX CAN FALAN OLAYLARINA Bİ BAK
     * ÇOK GARİP GELDİ ANLAMADIM. TAM OLARAK NASIL OLMASI GEREKİYOR
     * MAX CAN ALINDIĞINDA CURRENT ARTACAK MI?
     * ARTACAKSA HER ZAMAN MI YOKSA SADECE CAN FULL OLDUĞUNDA MI
     */
    
    
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


    public void IncreaseHealth(float damage)
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
    
    public void AddMaximumHealth(float health)
    {
        // Önce maxHealth'i artır
        maxHealth += health;

        // Eğer currentHealth zaten full ise, onu da artır
        if (currentHealth == maxHealth - health)
        {
            currentHealth = maxHealth;
        }
    }
    
    // bi yerde lazım olur belki
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public void AddHealthRegenAmount(float health)
    {
        healthRegenAmount = Mathf.Clamp(healthRegenAmount+health, 0, maxHealth);
    }

    public void AddHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);
    }
  
    public void AddHealthRegenRate(float newRate)
    {
        healthRegenRate = Mathf.Clamp(healthRegenRate+newRate, 0, maxHealth);
    }
    public void AddCurrentHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);
    }
    
    // Reducelar
    public void ReduceCurrentHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth - health, 0, maxHealth);
    }
    
    public void ReduceMaximumHealth(float health)
    {
        // Max health azalt
        maxHealth = Mathf.Max(0, maxHealth - health);

        // Eğer currentHealth yeni maxHealth'ten büyükse, onu maxHealth'e ayarla
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
    
    
    // Setter Fonksiyonları - HeroStats altında

    // Maximum Health Setter
    public void SetMaximumHealth(float health)
    {
        maxHealth = health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // Health Regen Amount Setter
    public void SetHealthRegenAmount(float health)
    {
        healthRegenAmount = Mathf.Clamp(health, 0, maxHealth);
    }

    // Health Setter
    public void SetCurrentHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
    }

    // Health Regen Rate Setter
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
    
}

