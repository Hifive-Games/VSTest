using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroExperience", menuName = "Experience System/HeroExperience")]
public class HeroExperienceLevelSO : ScriptableObject
{
    public int[] experienceRequired; // Her seviyeye geçmek için gereken XP miktarları
}
