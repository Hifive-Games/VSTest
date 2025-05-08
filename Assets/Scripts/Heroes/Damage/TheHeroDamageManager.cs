using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TheHeroDamageManager : MonoBehaviour
{
    private float armor=0;


    public void TakeDamage(float damage)
    {
        armor = TheHero.Instance.GetArmor();
        TheHero.Instance.IncreaseHealth(CalculateDamage(damage));
    }
    
    private float CalculateDamage(float baseDamage)
    {
        if (armor >= 0)
        {
            baseDamage = baseDamage * (100 /(100 + armor));
            return baseDamage;
        }
        // Zırh negatifse hasarı artır
        else
        {
            baseDamage = baseDamage * (100 / (100 - armor));
            return baseDamage;
        }
        
    }
}
