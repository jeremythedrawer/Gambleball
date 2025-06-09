using UnityEditor;
using UnityEngine;
using System;

[CustomPropertyDrawer(typeof(DayGameMode))]
public class DayGameModeDrawer : PropertyDrawer
{
    private static readonly Type[] ModeTypes = new Type[]
    {
        typeof(LivesData),
        typeof(TimerData),
        typeof(MoneyballData)
    };

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        SerializedProperty modeDataProp = property.FindPropertyRelative("modeData");
        float modeDataHeight = EditorGUI.GetPropertyHeight(modeDataProp, true);

        int drawerCount = 3;
        return (lineHeight + spacing) * drawerCount + modeDataHeight + spacing;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float y = position.y;

        var modeProp = property.FindPropertyRelative("Mode");
        var targetScoreProp = property.FindPropertyRelative("targetScore");
        var birdBoolProp = property.FindPropertyRelative("bird");
        var modeDataProp = property.FindPropertyRelative("modeData");

        // Draw each line
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), modeProp);
        y += lineHeight + spacing;

        EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), targetScoreProp);
        y += lineHeight + spacing;

        EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), birdBoolProp);
        y += lineHeight + spacing;

        // Determine expected data type based on enum
        GameMode selectedMode = (GameMode)modeProp.enumValueIndex;
        Type expectedType = ModeTypes[(int)selectedMode];

        if (modeDataProp.managedReferenceValue == null || modeDataProp.managedReferenceValue.GetType() != expectedType)
        {
            modeDataProp.managedReferenceValue = Activator.CreateInstance(expectedType);
        }

        float modeDataHeight = EditorGUI.GetPropertyHeight(modeDataProp, true);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, modeDataHeight), modeDataProp, new GUIContent("Data"), true);

        EditorGUI.EndProperty();
    }
}
