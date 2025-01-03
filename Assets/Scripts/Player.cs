using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public static Player Instance;

    public int BaseHealth { get; private set; }
    public int Health { get; set; }
    private List<Upgrade> appliedUpgrades;

    public int ExperienceToNextLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            BaseHealth = 100;
            Health = BaseHealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddExperience(int experience)
    {
        // Add experience logic here
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            // Handle player death
        }
    }
}