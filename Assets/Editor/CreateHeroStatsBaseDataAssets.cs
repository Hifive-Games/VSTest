#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

public static class CreateHeroStatsBaseDataAssets
{
    [MenuItem("Assets/Create/Generate All HeroStatsBaseData Assets")]
    public static void CreateAllHeroStatsBaseData()
    {
        // Assembly içerisindeki tüm HeroStatsBaseData türeyen sınıfları bulun
        var heroStatsBaseDataTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(HeroStatsBaseData)) && !type.IsAbstract)
            .ToArray();

        // Her sınıf için ScriptableObject oluşturun
        foreach (var type in heroStatsBaseDataTypes)
        {
            CreateAsset(type);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateAsset(Type type)
    {
        var asset = ScriptableObject.CreateInstance(type);
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if (!AssetDatabase.IsValidFolder(path))
        {
            path = System.IO.Path.GetDirectoryName(path);
        }

        string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{path}/{type.Name}.asset");
        AssetDatabase.CreateAsset(asset, assetPath);
        EditorUtility.SetDirty(asset);
    }
}
#endif