using UnityEditor;
using UnityEngine;

public class ActiveUpgradeBaseDataPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string assetPath in importedAssets)
        {
            if (assetPath.EndsWith(".asset"))
            {
                // Asset'i yükle
                var obj = AssetDatabase.LoadAssetAtPath<ActiveUpgradeBaseData>(assetPath);
                if (obj != null && (obj.rareValues == null || obj.rareValues.Count == 0))
                {
                    obj.ResetRareValues();
                    
                    // Değişiklikleri kaydet
                    EditorUtility.SetDirty(obj);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}