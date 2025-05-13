using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TheHeroMovementNewInput))]
[RequireComponent(typeof(TheHeroHealth))]
[RequireComponent(typeof(TheHeroLuck))]
[RequireComponent(typeof(TheHeroDamageManager))]
[RequireComponent(typeof(TheHeroArmor))]
[RequireComponent(typeof(TheHeroInteraction))]
[RequireComponent(typeof(TheHeroExperienceManager))]

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
    public virtual void SetArmor(float newRate)
    {
        GetComponent<TheHeroArmor>().SetArmor(newRate);
    }
    public virtual void SetBuffEffectScaler(float newRate)
    {
       GetComponent<TheHeroInteraction>().SetBuffEffectScaler(newRate);
    }
    public virtual void SetDeBuffEffectScaler(float newRate)
    {
        GetComponent<TheHeroInteraction>().SetDeBuffEffectScaler(newRate);
    }

    public abstract void SetAttackSpeed(float newRate);
    public abstract void SetAttackRange(float newRate);
    public abstract void SetAttackSize(float newRate);
    public abstract void SetAttackAmount(float newRate);
    public abstract void SetAttackDamage(float newRate);
    
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
    public virtual void AddArmor(float newRate)
    {
        GetComponent<TheHeroArmor>().AddArmor(newRate);
    }
    public virtual void AddBuffEffectScaler(float newRate)
    {
        GetComponent<TheHeroInteraction>().AddBuffEffectScaler(newRate);
    }
    public virtual void AddDeBuffEffectScaler(float newRate)
    {
        GetComponent<TheHeroInteraction>().AddDeBuffEffectScaler(newRate);
    }
    
    //Reducelar
    public virtual void ReduceCurrentHealth(float newRate)
    {
        GetComponent<TheHeroHealth>().ReduceCurrentHealth(newRate);
    }
    public virtual void ReduceLuck(float newRate)
    {
        GetComponent<TheHeroLuck>().ReduceLuck(newRate);
    }
    public virtual void ReduceMaximumHealth(float newRate)
    {
        GetComponent<TheHeroHealth>().ReduceMaximumHealth(newRate); // Max can artınca current can da artıyo (?)
    }
    public virtual void ReduceMovementSpeed(float value)
    {
        GetComponent<TheHeroMovementNewInput>().ReduceMoveSpeed(value);
    }
    
    //Getler
    public virtual float GetLuck()
    {
        return GetComponent<TheHeroLuck>().GetLuck();
    }
    public virtual float GetArmor()
    {
        return GetComponent<TheHeroArmor>().GetArmor();
    }
    
    //Increase
    public virtual void IncreaseHealth(float newRate)
    {
        GetComponent<TheHeroHealth>().IncreaseHealth(newRate);
    }
    
    // Add Hero Upgradeler
    public abstract void AddAttackSpeed(float newRate);
    public abstract void AddAttackRange(float newRate);
    public abstract void AddAttackSize(float newRate);
    public abstract void AddAttackAmount(float newRate);
    public abstract void AddAttackDamage(float newRate);

    // Reduce Hero Upgradeler
    public abstract void ReduceAttackSpeed(float newRate);


}
