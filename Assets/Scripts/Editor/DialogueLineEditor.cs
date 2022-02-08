using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static DialogueUtil;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(DialogueLine))]
public class DialogueLineDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        SerializedProperty sections = property.FindPropertyRelative("sections");

        for (int i = 0; i < sections.arraySize; i++)
        {

        }

        

        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("sections"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("portraitOverride"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("overflow"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        // Create property fields.
        var amountField = new PropertyField(property.FindPropertyRelative("sections"));
        var unitField = new PropertyField(property.FindPropertyRelative("portraitOverride"));
        var nameField = new PropertyField(property.FindPropertyRelative("overflow"), "Overflow?");

        // Add fields to the container.
        container.Add(amountField);
        container.Add(unitField);
        container.Add(nameField);

        return container;
    }
}
