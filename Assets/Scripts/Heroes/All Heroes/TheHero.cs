using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TheHeroMovementNewInput))]
[RequireComponent(typeof(TheHeroHealth))]

public abstract class TheHero : MonoBehaviourSingleton<TheHero>
{
    public virtual void SetMovementSpeed(float value)
    {
        GetComponent<TheHeroMovementNewInput>().SetMoveSpeed(value);
    }
    public virtual void SetAttackSpeed(float newRate)
    {
        Debug.LogWarning("SetSpawnRate fonksiyonu implement edilmelidir.");
    }
    public virtual void SetAttackRange(float newRate)
    {
        Debug.LogWarning("SetAttackRange fonksiyonu implement edilmelidir.");
    }
    public virtual void SetAttackSize(float newRate)
    {
        Debug.LogWarning("SetSpawnRate fonksiyonu implement edilmelidir.");
    }
    public virtual void SetAttackAmount	(float newRate)
    {
        Debug.LogWarning("SetSpawnRate fonksiyonu implement edilmelidir.");
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
}
