using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  HeroStatsBaseData : ScriptableObject
{
   [HideInInspector] public float value; // birisi değiştirmesin diye
   public abstract void ApplyStat(HeroBaseData hero);
}
