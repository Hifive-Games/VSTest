using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RareValue))]
public class RareValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Bir satırlık alanı hesapla
        var lineHeight = EditorGUIUtility.singleLineHeight;
        var currentRect = new Rect(position.x, position.y, position.width, lineHeight);

        // rareLevel alanını sadece okunabilir yap
        var rareLevelProperty = property.FindPropertyRelative("rareLevel");
        EditorGUI.LabelField(currentRect, "Rare Level", rareLevelProperty.enumDisplayNames[rareLevelProperty.enumValueIndex]);

        // Diğer alanları düzenlenebilir şekilde çiz
        currentRect.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("value"));

        currentRect.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("baseProbability"));

        currentRect.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("luckFactor"));

        EditorGUI.EndProperty();
        
    }
/*
 * BURADA 4 YAZIYOR YA ONU DA DEĞİŞTİRMEN LAZIM !
 */
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 4 satır: rareLevel + value + baseProbability + luckFactor
        return 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
    }
}