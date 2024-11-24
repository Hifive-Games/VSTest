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

        xpBar.SetLevel(level);
        xpBar.SetMaxXP(experienceToNextLevel);
        xpBar.ResetXP();
    }

    public bool StillNeedToLevelUp()
    {
        return experience >= experienceToNextLevel;
    }
}
