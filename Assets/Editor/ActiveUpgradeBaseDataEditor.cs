using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActiveUpgradeBaseData), true)]
public class ActiveUpgradeBaseDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // ScriptableObject referansı al
        var targetObject = (ActiveUpgradeBaseData)target;

        // rareValues hariç diğer alanları otomatik çiz
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true); // İlk alanı al (genelde "m_Script")
        do
        {
            // Eğer "rareValues" ise atla
            if (iterator.name == "rareValues")
                continue;

            EditorGUILayout.PropertyField(iterator, true);
        } while (iterator.NextVisible(false));

        // rareValues'ü özel olarak çiz
        SerializedProperty rareValues = serializedObject.FindProperty("rareValues");
        EditorGUILayout.LabelField("Rare Values", EditorStyles.boldLabel);

        for (int i = 0; i < rareValues.arraySize; i++)
        {
            SerializedProperty item = rareValues.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(item, new GUIContent($"Rare Value {i + 1}"), true);
        }

        // Düğme ile CreateOrReset çağır
        if (GUILayout.Button("Reset Rare Values"))
        {
            // CreateOrReset'i çağır
            targetObject.CreateOrReset();

            // Değişiklikleri işaretle ve görünümü güncelle
            EditorUtility.SetDirty(targetObject); // ScriptableObject'i kirli olarak işaretle
            serializedObject.Update(); // SerializedProperty'yi güncelle
            Repaint(); // Editor'ü yeniden çiz
        }

        serializedObject.ApplyModifiedProperties();
    }
}