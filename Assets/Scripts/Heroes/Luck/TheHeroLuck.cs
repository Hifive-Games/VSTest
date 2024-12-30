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
        Debug.LogError("Gelen değer: "+value);
        luck = value+luck;
        Debug.LogError("Son değer: "+luck);
    }
    public void ReduceLuck(float value)
    {
        luck =luck- value;
    }
}
