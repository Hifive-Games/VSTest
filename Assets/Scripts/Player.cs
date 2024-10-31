using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;
    public PlayerStats playerStats;
    public XPBar xpBar;

    public bool isInvincible = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerStats = new PlayerStats(100, 0, CharaterMovement.Instance.currentSpeed);
    }

    //we will add experience to the player until the experience is NOT enough to level up, then we will set xpBar and other things.

    public void AddExperience(int xp)
    {
        experience += xp;
        xpBar.AddXP(experience);
        if(StillNeedToLevelUp())
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = (int)(experienceToNextLevel * 1.1f);
        
        GameManager.Instance.LevelUp();
        LevelUpPanel.Instance.LevelUp();

        xpBar.SetLevel(level);
        xpBar.SetMaxXP(experienceToNextLevel);
        xpBar.ResetXP();
    }

    public bool StillNeedToLevelUp()
    {
        return experience >= experienceToNextLevel;
    }

    public void TakeDamage(int damage)
    {
        InterfaceManager.Instance.UpdateHealthText(playerStats.health);
        if (isInvincible)
        {
            return;
        }
        playerStats.health -= damage;
        if (playerStats.health <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(BecomeInvincible());
    }

    private IEnumerator BecomeInvincible()
    {
        MeshRenderer _renderer = GetComponent<MeshRenderer>();
        Color _orjMat = _renderer.material.color;
        _renderer.material.color = Color.red;
        isInvincible = true;
        yield return new WaitForSeconds(.2f);
        isInvincible = false;
        _renderer.material.color = _orjMat;
        
    }
    public void Die()
    {
        Debug.Log("Player died!");
        GameManager.Instance.GameOver();
    }
}
[Serializable]
public class PlayerStats
{
    public int health;
    public int armor;
    public float speed;

    public PlayerStats(int health, int armor, float speed)
    {
        this.health = health;
        this.armor = armor;
        this.speed = speed;
    }
}
