using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheHeroLuck : MonoBehaviour
{
    public float luck=0;

    public void SetLuck(float value)
    {
        luck = value;
    }
    public float GetLuck()
    {
        return luck;
    }
    public void AddLuck(float value)
    {
        luck =+ value;
    }
    public void ReduceLuck(float value)
    {
        luck =luck- value;
    }
}
