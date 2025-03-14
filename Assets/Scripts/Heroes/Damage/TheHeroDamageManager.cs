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
            // Zırh pozitifse hasarı azalt
            return baseDamage * (1 - Mathf.Clamp01(armor / 100f)); // Zırh oranını % olarak ele al
			// baseDamage = baseDamage * (100 /(100 + armor));
        }
        // Zırh negatifse hasarı artır
        return baseDamage * (1 + Mathf.Abs(armor) / 100f);
        
    }
}
