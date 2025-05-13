using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RareLevelColorData", menuName = "UI/RareLevelColorData")]
public class RareLevelColorData : ScriptableObject
{
    [Serializable]
    public class RareColorPair
    {
        public RareLevel rareLevel;
        public Color color = new Color(1, 1, 1, 50f);
    }

    [HideInInspector] public List<RareColorPair> rareColors = new();

    public Color GetColor(RareLevel level)
    {
        var found = rareColors.Find(pair => pair.rareLevel == level);
        return found != null ? found.color : Color.white;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        var enumValues = Enum.GetValues(typeof(RareLevel));

        // Eksik olanları ekle
        foreach (RareLevel level in enumValues)
        {
            if (!rareColors.Exists(x => x.rareLevel == level))
                rareColors.Add(new RareColorPair { rareLevel = level });
        }

        // Silinmiş enumları temizle
        rareColors.RemoveAll(x => !Enum.IsDefined(typeof(RareLevel), x.rareLevel));
    }
#endif
}