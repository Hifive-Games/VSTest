using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RareValue))]
public class RareValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Bir satırlık alanı hesapla
        float lineHeight = EditorGUIUtility.singleLineHeight;
        Rect currentRect = new Rect(position.x, position.y, position.width, lineHeight);

        // rareLevel alanını çiz
        var rareLevelProperty = property.FindPropertyRelative("rareLevel");
        EditorGUI.LabelField(currentRect, "Rare Level", rareLevelProperty.enumDisplayNames[rareLevelProperty.enumValueIndex]);

        // Diğer alanları çiz
        currentRect.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("value"));

        currentRect.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("baseProbability"));

        currentRect.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("luckFactor"));

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 4 satır: rareLevel + value + baseProbability + luckFactor
        return 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
    }
}