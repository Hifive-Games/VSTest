using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TheHeroMovementNewInput))]
[RequireComponent(typeof(TheHeroHealth))]
[RequireComponent(typeof(TheHeroLuck))]


public abstract class TheHero : MonoBehaviourSingleton<TheHero>
{
    //HeroStats
    public virtual void SetCurrentHealth(float newRate)
    {
        GetComponent<TheHeroHealth>().SetCurrentHealth(newRate);
    } 
    public virtual void SetMovementSpeed(float value)
    {
        GetComponent<TheHeroMovementNewInput>().SetMoveSpeed(value);
    }
    public virtual void SetMaximumHealth(float newRate)
    {
        GetComponent<TheHeroHealth>().SetMaximumHealth(newRate);
    }
    public virtual void SetHealthRegenAmount(float newRate)
    {
        GetComponent<TheHeroHealth>().SetHealthRegenAmount(newRate);
    }
    public virtual void SetHealthRegenRate(float newRate)
    {
        GetComponent<TheHeroHealth>().SetHealthRegenRate(newRate);
    }
    public virtual void SetLuck(float newRate)
    {
        GetComponent<TheHeroLuck>().SetLuck(newRate);
    }

  
    public abstract void SetAttackSpeed(float newRate);
    public abstract void SetAttackRange(float newRate);
    public abstract void SetAttackSize(float newRate);
    public abstract void SetAttackAmount(float newRate);
    
    
    // Diğer Upgradeler
    public virtual void AddMovementSpeed(float value)
    {
        GetComponent<TheHeroMovementNewInput>().AddMoveSpeed(value);
    }
    public virtual void AddMaximumHealth(float newRate)
    {
        GetComponent<TheHeroHealth>().AddMaximumHealth(newRate); // Max can artınca current can da artıyo (?)
    }
    public virtual void AddHealthRegenAmount(float newRate)
    {
        GetComponent<TheHeroHealth>().AddHealthRegenAmount(newRate);
    }
    public virtual void AddHealthRegenRate(float newRate)
    {
        GetComponent<TheHeroHealth>().AddHealthRegenRate(newRate);
    }
    public virtual void AddCurrentHealth(float newRate)
    {
        GetComponent<TheHeroHealth>().AddCurrentHealth(newRate);
    }
    public virtual void AddLuck(float newRate)
    {
        GetComponent<TheHeroLuck>().AddLuck(newRate);
    }
    public virtual float GetLuck()
    {
        return GetComponent<TheHeroLuck>().GetLuck();
    }
    // Upgradeler
    public abstract void AddAttackSpeed(float newRate);
    public abstract void AddAttackRange(float newRate);
    public abstract void AddAttackSize(float newRate);
    public abstract void AddAttackAmount(float newRate);
    
    
}
