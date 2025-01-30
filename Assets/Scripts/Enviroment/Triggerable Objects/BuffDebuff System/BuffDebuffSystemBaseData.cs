using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BuffDebuffSystemBaseData : ScriptableObject
{
    public string effectName;
    public string description;
    public float valueOfBuffOrDebuff;

    public float HeroBuffEffectScaler = 1;
    public float HeroDebuffEffectScaler = 1;

    private float BuffEffectScaler = 1;
    private float DebuffEffectScaler = 1;

    public void SetHeroBuffEffectScaler( float value)
    {
        Debug.LogError("HeroBuffEffectScaler:"+HeroBuffEffectScaler);
        HeroBuffEffectScaler = value;
    }
    public void SetHeroDebuffEffectScaler( float value)
    {
        HeroDebuffEffectScaler = value;
        Debug.LogError("HeroDebuffEffectScaler:"+HeroDebuffEffectScaler);
    }

    protected float GetBuffValue()
    {
        return HeroBuffEffectScaler * BuffEffectScaler * valueOfBuffOrDebuff;
    }
    protected float GetDebuffValue()
    {
        return HeroDebuffEffectScaler * DebuffEffectScaler * valueOfBuffOrDebuff;
    }
    public abstract void ApplyBuffDebuffSystem();
}
