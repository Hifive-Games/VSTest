using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ActiveUpgradeBaseData))]
public class ActiveUpgradeBaseDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Bu editördeki hedef nesne
        ActiveUpgradeBaseData upgradeData = (ActiveUpgradeBaseData)target;

        // rareValues listesini Serializing etmek
        serializedObject.Update();

        // rareValues listesini gösterelim
        SerializedProperty rareValuesProp = serializedObject.FindProperty("rareValues");

        // rareValues boyutunu değiştirmeyi engellemek için listeyi readonly yapıyoruz
        EditorGUI.BeginDisabledGroup(true);  // boyut değiştirmeyi engelle
        EditorGUILayout.PropertyField(rareValuesProp, true); // listeyi düzenlenebilir halde göster
        EditorGUI.EndDisabledGroup();

        // Diğer verileri düzenlemek için varsayılan gösterimi çağırıyoruz
        serializedObject.ApplyModifiedProperties();

        // Default Inspector ekleyebiliriz, böylece diğer alanlar düzenlenebilir
        DrawDefaultInspector();
    }
}