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
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        Rect currentRect = new Rect(position.x, position.y, position.width, lineHeight);

        // rareLevel alanını yalnızca okunabilir şekilde çiz
        var rareLevelProperty = property.FindPropertyRelative("rareLevel");
        EditorGUI.LabelField(currentRect, "Rare Level", rareLevelProperty.enumDisplayNames[rareLevelProperty.enumValueIndex]);

        // Diğer alanları dinamik olarak çiz
        currentRect.y += lineHeight + spacing;
        SerializedProperty iterator = property.Copy();
        iterator.NextVisible(true); // İlk alt alanı al
        int depth = iterator.depth;

        do
        {
            // rareLevel atla çünkü zaten çizildi
            if (iterator.name == "rareLevel")
                continue;

            EditorGUI.PropertyField(currentRect, iterator, true);
            currentRect.y += lineHeight + spacing;

        } while (iterator.NextVisible(false) && iterator.depth == depth);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // rareLevel hariç tüm alt alanların toplam yüksekliğini hesapla
        SerializedProperty iterator = property.Copy();
        iterator.NextVisible(true); // İlk alt alanı al
        int depth = iterator.depth;

        float totalHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // rareLevel için yükseklik
        do
        {
            if (iterator.depth != depth)
                break;

            if (iterator.name == "rareLevel")
                continue;

            totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        } while (iterator.NextVisible(false));

        return totalHeight - EditorGUIUtility.standardVerticalSpacing; // Son boşluğu çıkar
    }
}
