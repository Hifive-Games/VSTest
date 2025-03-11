using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BuffDebuffSystemBaseData : ScriptableObject
{
    public string effectName;
    public string description;
    public float valueOfBuffOrDeBuff;

    private float HeroBuffEffectScaler = 1;
    private float HeroDeBuffEffectScaler = 1;

    private float BuffEffectScaler = 1;
    private float DeBuffEffectScaler = 1;

    public void SetHeroBuffEffectScaler( float value)
    {
        Debug.LogError("HeroBuffEffectScaler:"+HeroBuffEffectScaler);
        HeroBuffEffectScaler = value;
    }
    public void SetHeroDeBuffEffectScaler( float value)
    {
        HeroDeBuffEffectScaler = value;
        Debug.LogError("HeroDebuffEffectScaler:"+HeroDeBuffEffectScaler);
    }

    protected float GetBuffValue()
    {
        return HeroBuffEffectScaler * BuffEffectScaler * valueOfBuffOrDeBuff;
    }
    protected float GetDeBuffValue()
    {
        return HeroDeBuffEffectScaler * DeBuffEffectScaler * valueOfBuffOrDeBuff;
    }
    public abstract void ApplyBuffDeBuffSystem();
}
