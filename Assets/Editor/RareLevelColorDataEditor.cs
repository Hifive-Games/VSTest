using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RareLevelColorData))]
public class RareLevelColorDataEditor : Editor
{
    SerializedProperty rareColors;

    private void OnEnable()
    {
        rareColors = serializedObject.FindProperty("rareColors");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        RareLevelColorData data = (RareLevelColorData)target;

        EditorGUILayout.LabelField("Rare Level Colors", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // rareColors array veya listesi üzerinde döngü
        for (int i = 0; i < rareColors.arraySize; i++)
        {
            SerializedProperty element = rareColors.GetArrayElementAtIndex(i);

            SerializedProperty rareLevel = element.FindPropertyRelative("rareLevel");
            SerializedProperty color = element.FindPropertyRelative("color");

            // rareLevel değerini enum ismine çevirme
            string rareLevelName = Enum.GetName(typeof(RareLevel), rareLevel.intValue);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(rareLevelName, GUILayout.Width(120)); // enum ismi
            EditorGUILayout.PropertyField(color, GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("RareLevel enum'daki değerler otomatik olarak görünür.\nSadece renkler düzenlenebilir.", MessageType.Info);

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}