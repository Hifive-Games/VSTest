using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public static Player Instance;

    public int BaseHealth { get; private set; }
    public int Health { get; set; }
    private List<SpellUpgrade> appliedUpgrades;

    public int ExperienceToNextLevel;

    public int Level { get; private set; }
    public int Experience { get; private set; }

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
        Experience += experience;
        if (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        Experience -= ExperienceToNextLevel;
        ExperienceToNextLevel = Level * 100;
        BaseHealth += 10;
        Health = BaseHealth;
    }

    public void TakeDamage(int damage)
    {
        print("Player took " + damage + " damage");
        Health -= damage;
        if (Health <= 0)
        {
            print("Player died");
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(70, 70, 500, 50), "Player HP: " + Health + "/" + BaseHealth);
    }
}