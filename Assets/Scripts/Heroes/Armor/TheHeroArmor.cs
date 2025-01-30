using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheHeroArmor : MonoBehaviour
{
    private float armor = 0;

    public void SetArmor(float newRate)
    {
        armor = newRate;
    }

    public float GetArmor()
    {
        return armor;
    }

    public void AddArmor(float newRate)
    {
        armor = newRate + armor;
    }
}
