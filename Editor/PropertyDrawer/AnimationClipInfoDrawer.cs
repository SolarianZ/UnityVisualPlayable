using UnityEditor;
using UnityEngine;

namespace GBG.VisualPlayable.Editor.PropertyDrawer
{
    [CustomPropertyDrawer(typeof(AnimationClipInfo))]
    public class AnimationClipInfoDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var lineHeight = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;
            var offset = lineHeight + spacing;

            var clipRect = new Rect(position.x, position.y + offset * 0, position.width, lineHeight);
            EditorGUI.PropertyField(clipRect, property.FindPropertyRelative(nameof(AnimationClipInfo.Clip)));
            var speedRect = new Rect(position.x, position.y + offset * 1, position.width, lineHeight);
            EditorGUI.PropertyField(speedRect, property.FindPropertyRelative(nameof(AnimationClipInfo.Speed)));
            var timeRect = new Rect(position.x, position.y + offset * 2, position.width, lineHeight);
            EditorGUI.PropertyField(timeRect, property.FindPropertyRelative(nameof(AnimationClipInfo.Time)));
            var weightRect = new Rect(position.x, position.y + offset * 3, position.width, lineHeight);
            EditorGUI.PropertyField(weightRect, property.FindPropertyRelative(nameof(AnimationClipInfo.Weight)));

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 3;
        }
    }
}