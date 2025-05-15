using System;
using UnityEngine;

public abstract class BuffDebuffSystemBaseData : ScriptableObject
{
    public string effectName;
    public string description;
    public float valueOfBuffOrDeBuff;

    private float HeroBuffEffectScaler = 1;
    private float HeroDeBuffEffectScaler = 1;

    private float BuffEffectScaler = 1;
    private float DeBuffEffectScaler = 1;

    public enum BuffDebuffTextColor
    {
        Green,
        Red
    }

    [SerializeField] public BuffDebuffTextColor textColor = BuffDebuffTextColor.Green; // Default renk

    public void SetHeroBuffEffectScaler(float value)
    {
        Debug.LogError("HeroBuffEffectScaler:" + HeroBuffEffectScaler);
        HeroBuffEffectScaler = value;
    }

    public void SetHeroDeBuffEffectScaler(float value)
    {
        HeroDeBuffEffectScaler = value;
        Debug.LogError("HeroDebuffEffectScaler:" + HeroDeBuffEffectScaler);
    }

    protected float GetBuffValue()
    {
        return HeroBuffEffectScaler * BuffEffectScaler * valueOfBuffOrDeBuff;
    }

    protected float GetDeBuffValue()
    {
        return HeroDeBuffEffectScaler * DeBuffEffectScaler * valueOfBuffOrDeBuff;
    }

    public virtual string GetBuffDebuffText()
    {
        // Yüzdeyi renkli yapmak
        string percentageText = $"%{valueOfBuffOrDeBuff}";

        // Geri kalan metin
        string descriptionText = $"{description}";

        // Renk belirleme
        string colorCode = "#00FF00"; // Default Green

        switch (textColor)
        {
            case BuffDebuffTextColor.Green:
                colorCode = "#00FF00";
                break;
            case BuffDebuffTextColor.Red:
                colorCode = "#FF0000";
                break;
        }

        // Yüzdeyi renkli yap, geri kalan kısmı beyaz tut
        return $"<color={colorCode}>{percentageText}</color> {descriptionText}";
    }

    public abstract void ApplyBuffDeBuffSystem();
}
